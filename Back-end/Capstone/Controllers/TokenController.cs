using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
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

        public TokenController(IConfiguration config, UserManager<User> userManager, IPermissionService permissionService)
        {
            _config = config;
            _userManager = userManager;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post([FromBody]CredentialsVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = CheckClaimIdentity(credentials.UserName, credentials.Password);
            if (identity.Result == null) {
                return BadRequest("Invalid username or password.");
            }

            var tokenString = GenerateJSONWebToken(identity.Result);
            return Ok(tokenString);
        }

        private async Task<User>CheckClaimIdentity(string username, string password)
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
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("permissions", listPermission),
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