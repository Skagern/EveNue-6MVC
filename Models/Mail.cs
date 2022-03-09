using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Example
{
    internal class Mail
    {
        public async Task Execute(string To, string subject, string plainTextContent, string htmlContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("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");
            var client = new SendGridClient(apiKey);
            var to = new EmailAddress(To);
            htmlContent = "<strong>" + htmlContent +"</strong>";
            var from = new EmailAddress("test@example.com", "Example User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}