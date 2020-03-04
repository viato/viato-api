using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, IConfigurationSection section)
        {
            var options = section.Get<SendGridOptions>();
            services.Configure<SendGridOptions>(section);
            services.AddTransient<ISendGridClient, SendGridClient>(x => new SendGridClient(options.ApiKey));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            return services;
        }
    }
}
