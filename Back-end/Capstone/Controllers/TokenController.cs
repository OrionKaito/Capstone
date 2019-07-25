using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly INotificationService _notificationService;
        private readonly IRequestActionService _requestActionService;
        private readonly IRequestService _requestService;

        public TokenController(IConfiguration config, UserManager<User> userManager, IPermissionService permissionService, IRoleService roleService
            , IUserNotificationService userNotificationService, INotificationService notificationService, IRequestActionService requestActionService
            , IRequestService requestService)
        {
            _config = config;
            _userManager = userManager;
            _permissionService = permissionService;
            _roleService = roleService;
            _userNotificationService = userNotificationService;
            _notificationService = notificationService;
            _requestActionService = requestActionService;
            _requestService = requestService;
        }

        [HttpGet("GetRole")]
        public ActionResult<IEnumerable<String>> GetRole()
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var userInDb = _userManager.FindByIdAsync(userId).Result;
                if (userInDb == null) return BadRequest(WebConstant.UserNotExist);

                var role = _roleService.GetByUserID(userId);
                return Ok(role);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("User")]
        public ActionResult<TokenVM> PostUser([FromBody]CredentialsVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var identity = CheckClaimIdentity(credentials.UserName, credentials.Password);
                if (identity.Result == null)
                {
                    return BadRequest(WebConstant.InvalidUSer);
                }

                var role = _roleService.GetByUserID(identity.Result.Id);

                bool checkUser = false;

                if (role.Name.Equals(WebConstant.User) || role.Name.Equals(WebConstant.Staff))
                {
                    checkUser = true;
                }

                if (!checkUser)
                {
                    return BadRequest(WebConstant.AccessDined);
                }

                if (identity.Result.EmailConfirmed == false)
                {
                    return BadRequest(WebConstant.VerifyAccount);
                }

                if (identity.Result.IsDeleted == true)
                {
                    return BadRequest(WebConstant.BannedAccount);
                }

                var tokenString = GenerateJSONWebToken(identity.Result);
                TokenVM tokenVM = new TokenVM
                {
                    Role = role.Name,
                    Token = tokenString
                };
                return Ok(tokenVM);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Admin")]
        public ActionResult<string> PostAdmin([FromBody]CredentialsVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var identity = CheckClaimIdentity(credentials.UserName, credentials.Password);
                if (identity.Result == null)
                {
                    return BadRequest(WebConstant.InvalidUSer);
                }

                var role = _roleService.GetByUserID(identity.Result.Id);

                bool checkAdmin = false;

                if (role.Name.Equals(WebConstant.Admin))
                {
                    checkAdmin = true;
                }

                if (!checkAdmin)
                {
                    return BadRequest(WebConstant.AccessDined);
                }

                if (identity.Result.EmailConfirmed == false)
                {
                    return BadRequest(WebConstant.VerifyAccount);
                }

                if (identity.Result.IsDeleted == true)
                {
                    return BadRequest(WebConstant.BannedAccount);
                }

                var tokenString = GenerateJSONWebToken(identity.Result);
                return Ok(tokenString);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("TestLogin")]
        public async Task<ActionResult<TokenVM>> TestLogin([FromBody]LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var identity = CheckClaimIdentity(loginVM.UserName, loginVM.Password);
                if (identity.Result == null)
                {
                    return BadRequest(WebConstant.InvalidUSer);
                }
                var userToVerify = _userManager.FindByNameAsync(loginVM.UserName).Result;
                userToVerify.DeviceID = loginVM.DeviceID;

                var result = await _userManager.UpdateAsync(userToVerify);
                var userNotificationList = _userNotificationService.GetByUserIDAndIsSend(userToVerify.Id, false);

                string[] deviceTokens = new string[]
                {
                    userToVerify.DeviceID
                };
                foreach (var userNotification in userNotificationList)
                {
                    bool sent = false;

                    var notification = _notificationService.GetByID(userNotification.NotificationID);
                    var requestAction = _requestActionService.GetByID(notification.EventID);
                    var request = _requestService.GetByID(requestAction.RequestID);

                    if (notification.NotificationType == NotificationEnum.ReceivedRequest)
                    {
                        sent = await PushNotification.SendMessageAsync(deviceTokens, "Received Request", WebConstant.ReceivedRequestMessage + " from " + _userManager.FindByIdAsync(request.InitiatorID).Result.FullName);
                    }
                    else if (notification.NotificationType == NotificationEnum.CompletedRequest)
                    {
                        sent = await PushNotification.SendMessageAsync(deviceTokens, "Completed Request", WebConstant.CompletedRequestMessage);
                    }

                    userNotification.IsSend = true;
                    _userNotificationService.Save();
                }

                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

                var role = _roleService.GetByUserID(identity.Result.Id);

                bool checkUser = false;

                if (role.Name.Equals(WebConstant.User) || role.Name.Equals(WebConstant.Staff))
                {
                    checkUser = true;
                }

                if (!checkUser)
                {
                    return BadRequest(WebConstant.AccessDined);
                }

                if (identity.Result.EmailConfirmed == false)
                {
                    return BadRequest(WebConstant.VerifyAccount);
                }

                if (identity.Result.IsDeleted == true)
                {
                    return BadRequest(WebConstant.BannedAccount);
                }

                var tokenString = GenerateJSONWebToken(identity.Result);
                TokenVM tokenVM = new TokenVM
                {
                    Role = role.Name,
                    Token = tokenString
                };
                return Ok(tokenVM);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Logout")]
        public async Task<ActionResult<string>> Logout()
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var userInDb = _userManager.FindByIdAsync(userId).Result;
                userInDb.DeviceID = "";
                var result = await _userManager.UpdateAsync(userInDb);
                if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);
                return StatusCode(201, "Logout Success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task<User> CheckClaimIdentity(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            //Get user to verify
            var userToVerify = await _userManager.FindByNameAsync(username);

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return userToVerify;
            }

            return null;
        }

        private string GenerateJSONWebToken(User user)
        {
            var permissions = _permissionService.GetByUserID(user.Id);

            string listPermission = "";

            foreach (var item in permissions)
            {
                listPermission = listPermission + " " + item.ToString();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(WebConstant.Permissions , listPermission),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}