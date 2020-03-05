using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace Viato.Api.Notification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(SendGridOptions));
            var options = section.Get<SendGridOptions>();
            services.Configure<SendGridOptions>(section);
            services.AddTransient<ISendGridClient, SendGridClient>(x => new SendGridClient(options.ApiKey));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            return services;
        }
    }
}
