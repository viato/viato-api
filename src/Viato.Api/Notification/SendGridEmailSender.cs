using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Viato.Api.Notification
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridOptions _options;
        private readonly ISendGridClient _client;
        private readonly IEmailTemplateRenderer _renderer;

        public SendGridEmailSender(IOptionsMonitor<SendGridOptions> optionsAccessor, ISendGridClient client, IEmailTemplateRenderer renderer)
        {
            _options = optionsAccessor?.CurrentValue ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public async Task SendAsync(string email, string subject, string htmlBody)
        {
            var message = new SendGridMessage
            {
                MailSettings = new MailSettings
                {
                    BypassListManagement = new BypassListManagement
                    {
                        Enable = true,
                    },
                },
                From = new EmailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                HtmlContent = htmlBody,
            };

            message.AddTo(email);

            await _client.SendEmailAsync(message);
        }

        public async Task SendAsync<T>(string email, string subject, string template, T model)
        {
            var emailBody = await _renderer.RenderAsync(template, model);
            await SendAsync(email, subject, emailBody);
        }
    }
}
