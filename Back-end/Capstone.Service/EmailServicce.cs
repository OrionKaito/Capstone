using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Capstone.Service
{
    public interface IEmailService
    {
        Task SendMail(string to, string subject, string message, List<string> filePaths);
        string GenerateMessageSendConfirmCode(string username, string emailConfirmCode);
        string GenerateMessageApproveRequest(string userName, List<string> names, List<string> links);
        string GenerateTestMessage();
        string GenerateMessageTest(string userEmail, string fromUser, string workflowName, string workflowActionName, Dictionary<string, string> dynamicForm, Dictionary<string, string> buttons);
    }

    public class EmailServicce : IEmailService
    {
        public EmailServicce() { }

        public async Task SendMail(string to, string subject, string message, List<string> filePaths)
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

            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());

            //kiểm tra list file path empty
            if (filePaths.Any())
            {
                foreach (var filePath in filePaths)
                {
                    msg.Attachments.Add(new Attachment(currentDirectory + "\\" + filePath));
                }
            }

            smtp.Send(msg);
        }

        public string GenerateMessageSendConfirmCode(string userName, string emailConfirmCode)
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var fullPath = currentDirectory + "\\EmailTemplate\\ConfirmCode.html";
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
            var fullPath = currentDirectory + "\\EmailTemplate\\ApproveRequest.html";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Username}", userName);

            //Lấy template cho button
            fullPath = currentDirectory + "\\EmailTemplate\\Button.html";
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

        public string GenerateTestMessage()
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var fullPath = currentDirectory + "\\EmailTemplate\\Request.html";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }

        public string GenerateMessageTest(string userEmail, string fromUser, string workflowName, string workflowActionName, Dictionary<string, string> dynamicForm, Dictionary<string, string> buttons)
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var fullPath = currentDirectory + "\\EmailTemplate\\Request.html";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(fullPath))
            {
                body = reader.ReadToEnd();
            }
            
            //Lấy template cho dynamicform
            fullPath = currentDirectory + "\\EmailTemplate\\DynamicForm.html";
            string listForm = string.Empty;

            foreach (var item in dynamicForm)
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    listForm += reader.ReadToEnd();
                }
                listForm = listForm.Replace("{Key}", item.Key.ToString());
                listForm = listForm.Replace("{Value}", item.Value.ToString());
            }

            //Lấy template cho button
            fullPath = currentDirectory + "\\EmailTemplate\\Button.html";
            string listButton = string.Empty;

            foreach (var button in buttons)
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    listButton += reader.ReadToEnd();
                }
                listButton = listButton.Replace("{ButtonLink}", button.Key.ToString());
                listButton = listButton.Replace("{ButtonName}", button.Value.ToString());
            }


            body = body.Replace("{DynamicForm}", listForm);
            body = body.Replace("{ListButton}", listButton);
            body = body.Replace("{useremail}", userEmail);
            body = body.Replace("{fromuser}", fromUser);
            body = body.Replace("{workflowname}", workflowName);
            body = body.Replace("{workflowactionname}", workflowActionName);

            return body;
        }
    }
}
