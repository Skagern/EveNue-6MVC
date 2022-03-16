using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GruppNrSexMVC.Models
{
    internal class Example
    {
        //public async Task<Response> Execute()
        //{
        //    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("jesper.tobiasson@student.hv.se", "Example User");
        //    var subject = "Sending with SendGrid is Fun";
        //    var to = new EmailAddress("jesper.tobiasson@qrew.se", "Example User");
        //    var plainTextContent = "and easy to do anywhere, even with C#";
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //    return response;
        //}


        public async Task Execute(string To, string subject, string plainTextContent, string htmlContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var to = new EmailAddress(To);
            htmlContent = "<strong>" + htmlContent + "</strong>";
            var from = new EmailAddress("jesper.tobiasson@student.hv.se", "Kungen");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
