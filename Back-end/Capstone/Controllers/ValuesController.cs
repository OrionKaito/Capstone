using Capstone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ValuesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUSer = HttpContext.User;
            var permissions = currentUSer.Claims.FirstOrDefault(c => c.Type == Helper.WebConstant.Permissions).Value;
            var user = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return new string[] { permissions, user };
        }

        // GET api/values/5
        [HttpGet("SendEmail")]
        public ActionResult<string> SendEmail()
        {
            _emailService.SendMail("orionkaito@gmail.com", "Hello", "Hello world");
            return "success";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
