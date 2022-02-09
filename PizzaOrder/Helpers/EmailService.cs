using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using MimeKit;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace PizzaOrder.Helpers
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailJetSettings settings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(settings.ApiKey, settings.ApiSecret);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "wladnaz@ukr.net")
               .Property(Send.FromName, "Vlad")
               .Property(Send.Subject, subject)
               //.Property(Send.TextPart, )
               .Property(Send.HtmlPart, htmlMessage)
               .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", email}
                    }
               });
            await client.PostAsync(request);
        }
    }

}
