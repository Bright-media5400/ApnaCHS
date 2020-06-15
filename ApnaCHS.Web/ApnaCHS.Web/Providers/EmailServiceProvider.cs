using ApnaCHS.Application.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ApnaCHS.Web.Providers
{
    public class EmailServiceProvider : IIdentityMessageService
    {
        private EmailSettingViewModel _emailSetting = new EmailSettingViewModel()
        {
            emailAddress = "burhaniemailservices@gmail.com",
            password = "testemail@BT52!",
            outgoingMailServer = "smtp.gmail.com",
            smtpPort = 587
        };
        private string title = "";

        public EmailServiceProvider()
        {

        }

        public EmailServiceProvider(EmailSettingViewModel emailSetting)
        {
            _emailSetting = emailSetting;
        }

        public Task SendAsync(IdentityMessage message)
        {
            MailAddress from = new MailAddress(_emailSetting.emailAddress, title);
            MailAddress to = new MailAddress(message.Destination, null);

            MailMessage email = new MailMessage(from, to);

            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;

            var mailClient = new SmtpClient(_emailSetting.outgoingMailServer, _emailSetting.smtpPort)
            {
                Credentials = new NetworkCredential(_emailSetting.emailAddress, _emailSetting.password),
                EnableSsl = true
            };

            return mailClient.SendMailAsync(email);
        }

        public Task GenerateBills(string flatowner, string email, decimal amount, string month,int year)
        {
            IdentityMessage message = new IdentityMessage();

            title = "ApnaCHS.com";
            message.Body = string.Format("Hello {0},<br />Your bill has been generated for month " + month + "," + year + "., "
           + "<br />Amount: " + amount + "rs", flatowner);
            message.Subject = month + "," + year + " - Your recent month maintenance bill";
            message.Destination = email;

            return SendAsync(message);
        }

    }
}
