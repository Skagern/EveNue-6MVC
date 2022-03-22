using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GruppNrSexMVC.Models
{
    internal class Example
    {

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
        public async Task ExecuteAll(string subject, string plainTextContent, string htmlContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            List<MailListModel> email = new();
            HttpClient client1 = new HttpClient();
            var response1 = await client1.GetAsync("http://193.10.202.76/MailAPI/api/MailList/getmaillist");
            string jsonresponse = await response1.Content.ReadAsStringAsync();
            email = JsonConvert.DeserializeObject<List<MailListModel>>(jsonresponse);
            List<EmailAddress> adress = new List<EmailAddress>();
            foreach (MailListModel mailListModel in email)
            {
                adress.Add(new EmailAddress(mailListModel.Mailadress, ""));
            }



            var msg = new SendGridMessage()
            {
                From = new EmailAddress("jesper.tobiasson@student.hv.se", "Evenue"),
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = "<strong>" + htmlContent + "</strong>",
                Personalizations = new List<Personalization>
    {
         new Personalization
         {
              Tos = adress

         }
     }
            };

            var response = await client.SendEmailAsync(msg);
        }

        public async Task ExecuteWelcome(string To, string subject, string plainTextContent, string htmlContent)
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
