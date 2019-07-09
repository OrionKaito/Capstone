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

        public TokenController(IConfiguration config, UserManager<User> userManager, IPermissionService permissionService, IRoleService roleService)
        {
            _config = config;
            _userManager = userManager;
            _permissionService = permissionService;
            _roleService = roleService;
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

                IEnumerable<string> listRoleOfUser = _roleService.GetByUserID(identity.Result.Id);

                bool checkUser = false;

                foreach (var item in listRoleOfUser)
                {
                    if (item.Equals(WebConstant.User) || item.Equals(WebConstant.Staff))
                    {
                        checkUser = true;
                    }
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

                bool checkStaff = false;

                foreach (var item in listRoleOfUser)
                {
                    if (item.Equals(WebConstant.Staff))
                    {
                        checkStaff = true;
                    }
                }

                string role = checkStaff == true ? WebConstant.Staff : WebConstant.User;
                var tokenString = GenerateJSONWebToken(identity.Result);
                TokenVM tokenVM = new TokenVM
                {
                    Role = role,
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

                IEnumerable<string> listRoleOfUser = _roleService.GetByUserID(identity.Result.Id);

                bool checkAdmin = false;

                foreach (var item in listRoleOfUser)
                {
                    if (item.Equals(WebConstant.Admin))
                    {
                        checkAdmin = true;
                    }
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
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}