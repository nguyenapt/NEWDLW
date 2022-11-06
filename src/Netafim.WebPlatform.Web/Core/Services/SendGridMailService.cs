using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EPiServer.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Netafim.WebPlatform.Web.Core.Services
{
    public interface IEmailService
    {
        Task<Response> SendEmail(Message message);
    }
    public class SendGridMailService : IEmailService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SendGridMailService));
        private readonly IEmailSettings _emailSettings;

        public SendGridMailService(IEmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public Task<Response> SendEmail(Message message)
        {
            return Task.Run(() => SendEmailAsync(message));
        }

        private async Task<Response> SendEmailAsync(Message message)
        {
            if (string.IsNullOrEmpty(_emailSettings.SendGridApiKey) ||
                string.IsNullOrEmpty(_emailSettings.SendGridTemplateId))
            {
                var logData = new
                {
                    apiKey = _emailSettings.SendGridApiKey,
                    templateId = _emailSettings.SendGridTemplateId,
                    sender = _emailSettings.EmailSenderAddress
                };
                var jsonData = JsonConvert.SerializeObject(logData, Formatting.Indented);
                _logger.Information($"Sent data to SendGrid: {jsonData}");

                return new Response(HttpStatusCode.LengthRequired, new StringContent(jsonData), null);
            }

            var client = new SendGridClient(_emailSettings.SendGridApiKey);

            var msg = new SendGridMessage();

            msg.SetTemplateId(_emailSettings.SendGridTemplateId);
            msg.SetFrom(new EmailAddress(_emailSettings.EmailSenderAddress));
            msg.SetSubject(message.Subject);
            msg.AddTos(message.Tos);
            if (message.Ccs.Any())
            {
                msg.AddCcs(message.Ccs);
            }
            if (message.Bccs.Any())
            {
                msg.AddBccs(message.Bccs);
            }

            if (message.ContentSubstitutions != null && message.ContentSubstitutions.Any())
            {
                foreach (var substitution in message.ContentSubstitutions)
                {
                    msg.AddSubstitution(substitution.Key, substitution.Value);
                }
            }
            
            if (message.Attachments != null && message.Attachments.Any())
            {
                foreach (var attachment in message.Attachments)
                {
                    msg.AddAttachment(attachment.Key, attachment.Value);    
                }
            }

            return await client.SendEmailAsync(msg);
        }
    }

    public class Message
    {
        public Message()
        {
            Tos = new List<EmailAddress>();
            Ccs = new List<EmailAddress>();
            Bccs = new List<EmailAddress>();
        }
        public EmailAddress From { get; set; }
        public List<EmailAddress> Tos { get; set; }
        public List<EmailAddress> Ccs { get; set; }
        public List<EmailAddress> Bccs { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> ContentSubstitutions { get; set; }
        public Dictionary<string, string> Attachments { get; set; }
    }
}