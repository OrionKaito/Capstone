using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Capstone.Service
{
    public interface IEmailService
    {
        Task SendMail(string to, string subject, string message);
        string GenerateMessageSendConfirmCode(string username, string emailConfirmCode);
        string GenerateMessageApproveRequest(string userName, List<string> names, List<string> links);
    }

    public class EmailServicce : IEmailService
    {
        public EmailServicce() { }

        public async Task SendMail(string to, string subject, string message)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("dynamicworkflowteam@gmail.com", "dynamicworkflow12345678");
            smtp.EnableSsl = true;

            MailMessage msg = new MailMessage();
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;
            msg.To.Add(to);
            msg.From = new MailAddress("DynamicWorkFlow Team <dynamicworkflowteam@gmail.com>");
            smtp.Send(msg);
        }

        public string GenerateMessageSendConfirmCode(string userName, string emailConfirmCode)
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var fullPath = currentDirectory + ".Service\\EmailTemplate\\ConfirmCode.html";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Username}", userName);
            body = body.Replace("{EmailConfirmCode}", emailConfirmCode);
            return body;
        }

        public string GenerateMessageApproveRequest(string userName, List<string> names, List<string> links)
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var fullPath = currentDirectory + ".Service\\EmailTemplate\\ApproveRequest.html";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Username}", userName);

            //Lấy template cho button
            fullPath = currentDirectory + ".Service\\EmailTemplate\\Button.html";
            string listButton = string.Empty;

            for (int i = 0; i < names.Count; i++)
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    listButton += reader.ReadToEnd();
                }
                listButton = listButton.Replace("{Link}", links[i]);
                listButton = listButton.Replace("{Name}", names[i]);
            }

            body = body.Replace("{ListButton}", listButton);
            return body;
        }
    }
}
