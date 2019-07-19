using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Capstone.Service
{
    public interface IEmailService
    {
        Task SendMail(string to, string subject, string message);
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
    }
}
