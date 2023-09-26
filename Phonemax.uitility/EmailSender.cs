using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.uitility
{
    public class EmailSender : IEmailSender
    {
        private EmailSettings _emailsettings { get; }
        public EmailSender(IOptions<EmailSettings> emailsettings)
        {
            _emailsettings = emailsettings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }
        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toemail = string.IsNullOrEmpty(email) ?
                    _emailsettings.ToEmail : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailsettings.UserNameEmail, "My Email Name")
                };
                mail.To.Add(toemail);
                mail.CC.Add(_emailsettings.CcEmail);
                mail.Subject = "Shopping App" + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                using (SmtpClient smtp = new SmtpClient(_emailsettings.PrimaryDomain, _emailsettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailsettings.UserNameEmail, _emailsettings.UserNamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}
