using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;
        private readonly ISendGridClient _client;

        public SendGridEmailSender(
            IOptionsMonitor<SendGridOptions> optionsAccessor,
            ISendGridClient client)
        {
            _options = optionsAccessor?.CurrentValue ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task SendAsync(string email, string subject, string htmlBody)
        {
            var message = new SendGridMessage
            {
                MailSettings = new MailSettings
                {
                    BypassListManagement = new BypassListManagement
                    {
                        Enable = true
                    }
                },
                From = new EmailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                HtmlContent = htmlBody
            };

            message.AddTo(email);

            await _client.SendEmailAsync(message);
        }
    }
}
