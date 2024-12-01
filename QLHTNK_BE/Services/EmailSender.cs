using System.Net;
using System.Net.Mail;

namespace QLHTNK_BE.Services
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "ngothibaolinhnm2003@gmail.com";
            var pw = "ihtfmeaaasyxiidr";
            MailMessage mailmessage = new MailMessage();
            mailmessage.From = new MailAddress("BestSmileinfo@gmail.com");
            mailmessage.Subject = subject;
            mailmessage.To.Add(new MailAddress(email));
            mailmessage.Body = message;
            mailmessage.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            };
            return smtpClient.SendMailAsync(mailmessage);
        }
    }
}
