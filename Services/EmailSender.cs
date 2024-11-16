using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TimeBasedPreventiveMeasures.Services
{
    public class EmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailSender()
        {
            _smtpClient = new SmtpClient("smtp-relay.brevo.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("784365001@smtp-brevo.com", "qPkQUXa9BchRxt3N"),
                EnableSsl = true,
            };
            _fromAddress = "delthondev+dev@gmail.com";
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromAddress, toAddress, subject, body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
