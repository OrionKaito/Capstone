using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserGroupService _userGroupService;
        private readonly IRoleService _roleService;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager
            , IEmailService emailService
            , IUserService userService
            , IUserRoleService userRoleService
            , IUserGroupService userGroupService
            , IRoleService roleService
            , IGroupService groupService
            , IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _userService = userService;
            _userRoleService = userRoleService;
            _userGroupService = userGroupService;
            _roleService = roleService;
            _groupService = groupService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAccount([FromBody]RegistrationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userInDB = _userManager.FindByNameAsync(model.Email).Result;
            if (userInDB != null) return BadRequest(WebConstant.EmailExisted);

            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                ManagerID = model.ManagerID,
                IsDeleted = false,
            };

            Random random = new Random();
            user.EmailConfirmCode = random.Next(100001, 999999).ToString();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            else
            {
                try
                {
                    //Begin transaction
                    _userService.BeginTransaction();
                    //Role
                    foreach (var roleID in model.RoleIDs)
                    {
                        UserRole userRole = new UserRole
                        {
                            RoleID = roleID,
                            UserID = user.Id,
                        };
                        _userRoleService.Create(userRole);
                    }

                    //Group
                    foreach (var groupID in model.GroupIDs)
                    {
                        UserGroup userGroup = new UserGroup
                        {
                            GroupID = groupID,
                            UserID = user.Id,
                        };
                        _userGroupService.Create(userGroup);
                    }

                    //Send mail

                    await _emailService.SendMail(user.Email, "Activation Code to Verify Email Address", "Thank you for creating an account with DynamicWorkflow"
                        + "\n\nAccount name : "
                        + user.UserName
                        + "\n\nYour account will work but you must verify it by enter this code in our app"
                        + "\n\nYour Activation Code is : "
                        + user.EmailConfirmCode
                        + "\n\nThanks & Regards\nDynamicWorkFlow Team");

                    //End transaction
                    _userService.CommitTransaction();

                    return new OkObjectResult(WebConstant.AccountCreated);
                }
                catch (Exception e)
                {
                    _userService.RollBack();
                    await _userManager.DeleteAsync(user);
                    return BadRequest(e.Message);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string code, string email)
        {
            try
            {
                var userInDB = await _userManager.FindByEmailAsync(email);

                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                if (userInDB.EmailConfirmCode == code)
                {
                    userInDB.EmailConfirmed = true;
                    await _userManager.UpdateAsync(userInDB);
                }
                else
                {
                    return BadRequest(WebConstant.WrongCodeConfirm);
                }

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<RegistrationPaginVM> GetAccountsPagination(int? numberOfPage, int? NumberOfRecord)
        {
            try
            {
                var page = numberOfPage ?? 1;
                var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

                var users = _userManager.Users
                    .ToListAsync()
                    .Result
                    .Skip((page - 1) * count)
                    .Take(count)
                    .Select(u => new RegistrationVM
                    {
                        ID = u.Id,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        FullName = u.FullName,
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = _groupService.GetByID(g.GroupID).Name,
                                }),
                        Roles = _userRoleService.GetByUserID(u.Id)
                                .Select(r => new RoleVM
                                {
                                    ID = r.RoleID,
                                    Name = _roleService.GetByID(r.RoleID).Name,
                                }),
                        ManagerID = u.ManagerID,
                        ManagerName = string.IsNullOrEmpty(u.ManagerID) ? "" : _userManager.FindByIdAsync(u.ManagerID).Result.FullName,
                        IsDeleted = u.IsDeleted
                    });

                var data = new RegistrationPaginVM
                {
                    TotalPage = _userManager.Users.ToListAsync().Result.Count,
                    Accounts = users,
                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAccountByUserID")]
        public ActionResult<IEnumerable<RegistrationVM>> GetAccountByUserID(string ID)
        {
            try
            {

                var user = _userManager.Users
                    .Where(u => u.Id == ID)
                    .ToList()
                    .Select(u => new RegistrationVM
                    {
                        ID = u.Id,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        FullName = u.FullName,
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = _groupService.GetByID(g.GroupID).Name,
                                }),
                        Roles = _userRoleService.GetByUserID(u.Id)
                                .Select(r => new RoleVM
                                {
                                    ID = r.ID,
                                    Name = _roleService.GetByID(r.RoleID).Name,
                                }),
                        ManagerID = u.ManagerID,
                        ManagerName = string.IsNullOrEmpty(u.ManagerID) ? "" : _userManager.FindByIdAsync(u.ManagerID).Result.FullName,
                        IsDeleted = u.IsDeleted
                    });
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetProfile")]
        public ActionResult<IEnumerable<RegistrationVM>> GetProfile()
        {
            try
            {
                RegistrationVM result = new RegistrationVM();
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user = _userManager.Users
                    .Where(u => u.Id == userId)
                    .ToList()
                    .Select(u => new RegistrationVM
                    {
                        ID = u.Id,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        FullName = u.FullName,
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = _groupService.GetByID(g.GroupID).Name,
                                }),
                        Roles = _userRoleService.GetByUserID(u.Id)
                                .Select(r => new RoleVM
                                {
                                    ID = r.ID,
                                    Name = _roleService.GetByID(r.RoleID).Name,
                                }),
                        ManagerID = u.ManagerID
                    });

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateProfile")]
        public async Task<ActionResult> PutAccount(RegistrationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var currentUSer = HttpContext.User;
                var userID = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                userInDB.FullName = model.FullName;
                userInDB.DateOfBirth = model.DateOfBirth;
                userInDB.SecurityStamp = Guid.NewGuid().ToString();
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var userInDB = await _userManager.FindByEmailAsync(email);

                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                Random random = new Random();
                userInDB.EmailConfirmCode = random.Next(100001, 999999).ToString();
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                await _emailService.SendMail(email, "Request To Change Password", "Account name : "
                        + userInDB.UserName
                        + "\n\nYour Code is : "
                        + userInDB.EmailConfirmCode
                + "\n\nThanks & Regards\nDynamicWorkFlow Team");

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ConfirmForgotPassword")]
        public async Task<ActionResult> ConfirmForgotPassword(string code, string email, string password)
        {
            try
            {
                var userInDB = await _userManager.FindByEmailAsync(email);

                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                if (userInDB.EmailConfirmCode.Equals(code))
                {
                    var PasswordHash = new PasswordHasher<string>();

                    userInDB.PasswordHash = PasswordHash.HashPassword(userInDB.UserName, password);
                    var result = await _userManager.UpdateAsync(userInDB);

                    if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);
                }
                else
                {
                    return BadRequest(WebConstant.WrongCodeConfirm);
                }

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string password)
        {
            try
            {
                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                var PasswordHash = new PasswordHasher<string>();

                userInDB.PasswordHash = PasswordHash.HashPassword(userInDB.UserName, password);
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("PutByID")]
        public async Task<ActionResult> PutAccountByID([FromBody]RegistrationByIDUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _userService.BeginTransaction();

                var userInDB = _userManager.FindByIdAsync(model.ID).Result;
                if (userInDB == null) return BadRequest(WebConstant.NotFound);

                userInDB.ManagerID = model.ManagerID;
                userInDB.SecurityStamp = Guid.NewGuid().ToString();
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                //Delete Group
                var groups = _userGroupService.GetByUserID(model.ID);
                foreach (var group in groups)
                {
                    _userGroupService.Delete(group);
                }

                //Delete Role
                var roles = _userRoleService.GetByUserID(model.ID);
                foreach (var role in roles)
                {
                    _userRoleService.Delete(role);
                }

                //Add Role
                foreach (var roleID in model.RoleIDs)
                {
                    UserRole userRole = new UserRole
                    {
                        RoleID = roleID,
                        UserID = model.ID,
                    };
                    _userRoleService.Create(userRole);
                }

                //Add Group
                foreach (var groupID in model.GroupIDs)
                {
                    UserGroup userGroup = new UserGroup
                    {
                        GroupID = groupID,
                        UserID = model.ID,
                    };
                    _userGroupService.Create(userGroup);
                }
                //End transaction
                _userService.CommitTransaction();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                _userService.RollBack();
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ToggleBanAccount")]
        public async Task<ActionResult> ToggleBanAccount(string ID)
        {
            var userInDB = _userManager.FindByIdAsync(ID).Result;
            if (userInDB == null) return BadRequest(WebConstant.NotFound);

            try
            {
                if (userInDB.IsDeleted == true)
                {
                    userInDB.IsDeleted = false;
                }
                else
                {
                    userInDB.IsDeleted = true;
                }

                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}