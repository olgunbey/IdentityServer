using IdentityOrnek.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityOrnek.Services
{
    public class EMailService : IEmailService
    {
        private EmailSettings EmailSettings;
        public EMailService(IOptions<EmailSettings> options)
        {
            EmailSettings = options.Value;
        }
        public async Task SendResetEmail(string resetEmailLink, string tooEmail)
        {
            var smptClient = new SmtpClient();
            //info@olgun.com
            smptClient.Host = EmailSettings.Host!; //smtp.gmmail. -- domain name olgun.com
            smptClient.DeliveryMethod=SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587; // mail servisinin payllsılgı port. 8080    850
            smptClient.Credentials = new NetworkCredential(EmailSettings.Email, EmailSettings.Password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(EmailSettings.Email!);
            mailMessage.To.Add(tooEmail);
            mailMessage.Subject = "Localhost | Şifre sıfırlama";
            mailMessage.Body = $"<h4>Şifrenizi yenilemek için aşşağıdaki linke tıklayınız</h4> <p><a href='{resetEmailLink}'>Şifre yenileme</a> </p>";


            mailMessage.IsBodyHtml = true;
            await smptClient.SendMailAsync(mailMessage);
        }
    }
}
