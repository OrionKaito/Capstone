using Capstone.Helper;
using Capstone.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IRequestValueService _requestValueService;

        public ValuesController(IEmailService emailService, IRequestValueService requestValueService)
        {
            _emailService = emailService;
            _requestValueService = requestValueService;
        }

        [HttpGet("Test")]
        public ActionResult Test()
        {
            Dictionary<string, string> buttons = new Dictionary<string, string>();

            //Uri uriAddress = new Uri(UriPartial.Authority.ToString());
            //Console.WriteLine(uriAddress.Fragment);
            var domain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            //var domainName = HttpContext. ..GetLeftPart(UriPartial.Authority);
            buttons.Add("Accept", domain + "/api/Accounts");
            buttons.Add("Deny", "http://www.example.com");
            buttons.Add("Send to Mangaer", "http://www.example.com");



            List<string> names = new List<string>();
            List<string> links = new List<string>();
            foreach (var button in buttons)
            {
                names.Add(button.Key.ToString());
                links.Add(button.Value.ToString());
            }

            //string message = _emailService.GenerateMessageApproveRequest("Kiet", names, links);
            string message = _emailService.GenerateTestMessage();
            try
            {
                _emailService.SendMail("kevinz2014st@gmail.com", "Test", message);
            }
            catch (Exception e)
            {
                return BadRequest("Wrong " + e.Message);
            }
            return Ok(domain);
        }

        [HttpGet("TestNew")]
        public ActionResult TestNew()
        {
            var requestValue = _requestValueService.GetByRequestActionID(new Guid("e80dcf4a-da7d-4cfa-6b29-08d71742fd05"));

            Dictionary<string, string> dynamicform = new Dictionary<string, string>();

            foreach (var item in requestValue)
            {
                dynamicform.Add(item.Key, item.Value);
            }

            Dictionary<string, string> listButton = new Dictionary<string, string>();
            listButton.Add("https://www.google.com/", "Go To Google");
            listButton.Add("https://www.facebook.com/", "Go To Facebook");

            //Uri uriAddress = new Uri(UriPartial.Authority.ToString());
            //Console.WriteLine(uriAddress.Fragment);
            var domain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            
            

            //string message = _emailService.GenerateMessageApproveRequest("Kiet", names, links);
            string message = _emailService.GenerateMessageTest("kevinz2014st@gmail.com","locnt","Quy trình nghỉ học","Phòng đào tạo duyệt", dynamicform, listButton);
            try
            {
                _emailService.SendMail("kevinz2014st@gmail.com", "Test", message);
            }
            catch (Exception e)
            {
                return BadRequest("Wrong " + e.Message);
            }
            return Ok(domain);
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUSer = HttpContext.User;
            var permissions = currentUSer.Claims.FirstOrDefault(c => c.Type == Helper.WebConstant.Permissions).Value;
            var userId = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return new string[] { permissions, userId };
        }

        // GET api/values/5
        [HttpGet("SendEmail")]
        public ActionResult<string> SendEmail()
        {
            var template = "<style type=\"text/css\"> " +
                " body,html,.body { background: #f3f3f3 !important; } " +
                ".container.header { background: #f3f3f3;} " +
                ".body-border {border-top: 8px solid #663399;}" +
                "</style> <spacer size=\"16\"></spacer>" +
                "<container class=\"header\">" +
                "<row>" +
                "<columns>" +
                "<center>" +
                "<h1 class=\"text - center\">Welcome to Dynamic WorkFlow</h1>" +
                "</center>" +
                "<center>" +
                "<menu class=\"text - center\">" +
                //"<item href=\"#\">About</item>" +
                //"<item href=\"#\">Course List</item>" +
                //"<item href=\"#\">Campus Map</item>" +
                //"<item href=\"#\">Contact</item>" +
                "</menu>" +
                "</center>" +
                "</columns>" +
                "</row>" +
                "</container>" +
                "<container class=\"body-border\">" +
                "<row>" +
                "<columns>" +
                "<spacer size=\"32\">" +
                "</spacer>" +
                "<center>" +
                "<img src=\"https://doc-0s-as-docs.googleusercontent.com/docs/securesc/ha0ro937gcuc7l7deffksulhg5h7mbp1/2ffhedil82jau06t0e75110nq4svhlj0/1563501600000/05868357909278580689/*/1jrVKFISjf1cXIk3-tg9XoYCcgkRL-Dgm\">" +
                "</center>" +
                "<spacer size=\"16\">" +
                "</spacer>" +
                "<h4>Just one more step.</h4>" +
                "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Atque culpa vel architecto, perspiciatis eius cum autem quidem, sunt consequuntur, impedit dolor vitae illum nobis sint nihil aliquid? Assumenda, amet, officia.</p>" +
                "<center>" +
                "<menu>" +
                "<item href=\"#\">dynamicworkflow.com</item> " +
                "<item href=\"#\">Facebook</item> " +
                "<item href=\"#\">Twitter</item> " +
                "<item href=\"#\">(408)-555-0123</item>" +
                "</menu>" +
                "</center>" +
                "</columns>" +
                "</row>" +
                "<spacer size=\"16\"></spacer>" +
                "</container>";
            var test = "<h1>This is h1 </h1>";
            _emailService.SendMail("orionkaito@gmail.com", "Hello", template);
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

        // POST api/values
        [HttpPost("PushNotificationToDevice")]
        public async Task<bool> PushNotificationToDevice(PushNotificationModel model)
        {
            try
            {
                return await PushNotification.SendMessageAsync(model.registration_ids, model.title, model.body);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
