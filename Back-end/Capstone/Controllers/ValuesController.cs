using Capstone.Service;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IRequestValueService _requestValueService;
        private readonly IBackgroundJobClient _backgroundJob;
        private readonly IWebHookService _webHookService;
        private readonly IEncodeService _encodeService;

        public ValuesController(IEmailService emailService, IRequestValueService requestValueService, IBackgroundJobClient backgroundJob
            , IWebHookService webHookService, IEncodeService encodeService)
        {
            _emailService = emailService;
            _requestValueService = requestValueService;
            _backgroundJob = backgroundJob;
            _webHookService = webHookService;
            _encodeService = encodeService;
        }

        [HttpGet("Test")]
        public ActionResult Test()
        {
            Dictionary<string, string> buttons = new Dictionary<string, string>();

            //Uri uriAddress = new Uri(UriPartial.Authority.ToString());
            //Console.WriteLine(uriAddress.Fragment);
            var domain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            //var domainName = HttpContext. ..GetLeftPart(UriPartial.Authority);
            buttons.Add("Accept", "https://localhost:44327/home/index/2?name=kaka");
            buttons.Add("Deny", "http://www.example.com");
            buttons.Add("Send to Mangaer", "http://www.example.com");



            List<string> names = new List<string>();
            List<string> links = new List<string>();
            foreach (var button in buttons)
            {
                names.Add(button.Key.ToString());
                links.Add(button.Value.ToString());
            }

            string message = _emailService.GenerateMessageApproveRequest("Kiet", names, links);
            //string message = _emailService.GenerateTestMessage();
            try
            {
                _emailService.SendMail("orionkaito@gmail.com", "Test", message, new List<string>());
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
            listButton.Add("https://localhost:44327/home/index/2?name=kaka", "Accept");
            listButton.Add("https://www.facebook.com/", "Go To Facebook");

            //Uri uriAddress = new Uri(UriPartial.Authority.ToString());
            //Console.WriteLine(uriAddress.Fragment);
            var domain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";



            //string message = _emailService.GenerateMessageApproveRequest("Kiet", names, links);
            string message = _emailService.GenerateMessageTest("orionkaito@gmail.com", "locnt", "Quy trình nghỉ học", "Phòng đào tạo duyệt", dynamicform, null, listButton);
            try
            {
                _emailService.SendMail("orionkaito@gmail.com", "Test", message, new List<string>());
            }
            catch (Exception e)
            {
                return BadRequest("Wrong " + e.Message);
            }
            return Ok(domain);
        }

        [HttpGet("Encrypt")]
        public ActionResult TestHandfire(string encrypt)
        {
            return Ok(_encodeService.Encrypt(encrypt));
        }

        [HttpGet("Decrypt")]
        public ActionResult TestHand(string decrypt)
        {
            return Ok(_encodeService.Decrypt(decrypt));
        }

        //[HttpGet("TestWebHook")]
        //public async Task<HttpResponseMessage> TestWebHook(string url, string message)
        //{
        //    return await _webHookService.WebHook(url, message);
        //}

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
