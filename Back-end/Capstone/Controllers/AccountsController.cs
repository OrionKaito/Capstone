﻿using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
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
                    .Select(u => new RegistrationVM
                    {
                        ID = u.Id,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        FullName = u.FullName,
                        ImagePath = u.ImagePath == null ? "" : u.ImagePath,
                        Role = new RoleVM
                        {
                            ID = _userRoleService.GetByUserID(u.Id).RoleID,
                            Name = _userRoleService.GetByUserID(u.Id).Role.Name
                        },
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = g.Group.Name,
                                }),
                        LineManagerID = u.LineManagerID,
                        ManagerName = string.IsNullOrEmpty(u.LineManagerID) ? "" : u.LineManager.FullName,
                        IsDeleted = u.IsDeleted
                    }).Skip((page - 1) * count).Take(count);

                var data = new RegistrationPaginVM
                {
                    TotalRecord = _userService.Count(),
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
                        ImagePath = u.ImagePath == null ? "" : u.ImagePath,
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = g.Group.Name,
                                }),
                        Role = new RoleVM
                        {
                            ID = _userRoleService.GetByUserID(u.Id).RoleID,
                            Name = _userRoleService.GetByUserID(u.Id).Role.Name,
                        },
                        LineManagerID = u.LineManagerID,
                        ManagerName = string.IsNullOrEmpty(u.LineManagerID) ? "" : u.LineManager.FullName,
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
                        ImagePath = u.ImagePath == null ? "" : u.ImagePath,
                        Groups = _userGroupService.GetByUserID(u.Id)
                                .Select(g => new GroupVM
                                {
                                    ID = g.GroupID,
                                    Name = g.Group.Name,
                                }),
                        Role = new RoleVM
                        {
                            ID = _userRoleService.GetByUserID(u.Id).RoleID,
                            Name = _userRoleService.GetByUserID(u.Id).Role.Name,
                        },
                        LineManagerID = u.LineManagerID,
                        ManagerName = string.IsNullOrEmpty(u.LineManagerID) ? "" : u.LineManager.FullName,
                    });

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
                LineManagerID = model.LineManagerID,
                IsDeleted = false,
            };

            user.EmailConfirmCode = Utilities.RandomString(8);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            else
            {
                try
                {
                    //Begin transaction
                    _userService.BeginTransaction();
                    //Role
                    UserRole userRole = new UserRole
                    {
                        RoleID = model.RoleID,
                        UserID = user.Id,
                    };
                    _userRoleService.Create(userRole);

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

                    var message = _emailService.GenerateMessageSendConfirmCode(user.UserName, user.EmailConfirmCode);

                    await _emailService.SendMail(user.Email, "Activation Code to Verify Email Address", message, new List<string>());

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

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var userInDB = await _userManager.FindByEmailAsync(email);

                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                Random random = new Random();
                userInDB.EmailConfirmCode = Utilities.RandomString(8); ;
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);
                var message = _emailService.GenerateMessageSendConfirmCode(userInDB.UserName, userInDB.EmailConfirmCode);
                await _emailService.SendMail(email, "Request To Change Password", message, new List<string>());

                return Ok(WebConstant.Success);
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
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                if (newPassword.Length < 6)
                {
                    return BadRequest(WebConstant.PasswordToShort);
                }
                var PasswordHash = new PasswordHasher<string>();
                string currentPasswordHash = userInDB.PasswordHash;
                string oldPasswordHash = PasswordHash.HashPassword(userInDB.UserName, oldPassword);

                if (PasswordHash.VerifyHashedPassword(userInDB.UserName, currentPasswordHash, oldPassword) == 0)
                {
                    return BadRequest(WebConstant.WrongOldPassword);
                }

                userInDB.PasswordHash = PasswordHash.HashPassword(userInDB.UserName, newPassword);
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("PutAccountByAdmin")]
        public async Task<ActionResult> PutAccountByAdmin([FromBody]RegistrationByAdminUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Begin transaction
                _userService.BeginTransaction();

                var userInDB = _userManager.FindByIdAsync(model.ID).Result;
                if (userInDB == null) return BadRequest(WebConstant.NotFound);

                userInDB.LineManagerID = model.LineManagerID;
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
                var role = _userRoleService.GetByUserID(model.ID);
                _userRoleService.Delete(role);

                //Add Role
                UserRole userRole = new UserRole
                {
                    RoleID = model.RoleID,
                    UserID = model.ID,
                };
                _userRoleService.Create(userRole);

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

        [HttpPut("UpdateAvatar")]
        public async Task<ActionResult> UpdateAvatar(UpdateAvatar model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var currentUser = HttpContext.User;
                var userID = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest(WebConstant.UserNotExist);

                userInDB.ImagePath = model.ImagePath;
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