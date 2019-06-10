using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, IEmailService emailService,
            IUserService userService, IUserRoleService userRoleService, IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _userService = userService;
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userInDB = _userManager.FindByNameAsync(model.Email).Result;
            if (userInDB != null) return BadRequest("Email is existed!");

            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                CreateDate = DateTime.Now,
                DateOfBirth = model.DateOfBirth,
                IsDeleted = false,
            };

            Random random = new Random();
            user.EmailConfirmCode = random.Next(100001, 999999).ToString();

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            await _emailService.SendMail(user.Email, "Activation Code to Verify Email Address", "Thank you for creating an account with Gigshub"
                + "\n\nAccount name : "
                + user.UserName
                + "\n\nYour account will work but you must verify it by enter this code in our app"
                + "\n\nYour Activation Code is : "
                + user.EmailConfirmCode
                + "\n\nThanks & Regards\nDynamicWorkFlow Team");

            //_userRoleService.Create()

            return new OkObjectResult("Account created, please check your email!");
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string code, string email)
        {
            try
            {
                var userInDB = _userManager.FindByEmailAsync(email).Result;

                if (userInDB == null) return BadRequest("User is not exist");

                if (userInDB.EmailConfirmCode == code)
                {
                    userInDB.EmailConfirmed = true;
                    await _userManager.UpdateAsync(userInDB);
                }
                else
                {
                    return BadRequest("Wrong code! please try again");
                }

                return Ok("Success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<RegistrationVM>> GetAccounts()
        {
            try
            {
                List<RegistrationVM> result = new List<RegistrationVM>();
                var users = _userManager.Users.ToListAsync().Result;
                foreach (var item in users)
                {
                    result.Add(_mapper.Map<RegistrationVM>(item));
                }
                return Ok(result);
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
                var user = _userManager.FindByIdAsync(userId).Result;
                result = _mapper.Map<RegistrationVM>(user);
                return Ok(result);
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
                RegistrationVM result = new RegistrationVM();
                var user = _userManager.FindByIdAsync(ID).Result;
                result = _mapper.Map<RegistrationVM>(user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetAuthorizationByUserID")]
        public ActionResult<AuthorizationVM> GetAuthorizationByUserID(string ID)
        {
            try
            {
                Dictionary<string, IEnumerable<string>> data = _userService.GetAuthorizationByUserID(ID);
                IEnumerable<string> roles = data.GetValueOrDefault("role");
                IEnumerable<string> groups = data.GetValueOrDefault("group");
                IEnumerable<string> permissions = data.GetValueOrDefault("permission");
                AuthorizationVM result = new AuthorizationVM();
                result.Roles = roles;
                result.Groups = groups;
                result.Permissions = permissions;
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody]RegistrationUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var currentUSer = HttpContext.User;
                var userID = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var userInDB = _userManager.FindByIdAsync(userID).Result;
                if (userInDB == null) return BadRequest("ID not found!");

                userInDB.FullName = model.FullName;
                userInDB.DateOfBirth = model.DateOfBirth;
                var result = await _userManager.UpdateAsync(userInDB);

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ToggleBanAccount")]
        public async Task<ActionResult> ToggleBanAccount(string ID)
        {
            var userInDB = _userManager.FindByIdAsync(ID).Result;
            if (userInDB == null) return BadRequest("ID not found!");

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

                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}