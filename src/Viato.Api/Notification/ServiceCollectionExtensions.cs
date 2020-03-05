using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace Viato.Api.Notification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(opts =>
            {
                opts.ViewLocationFormats.Clear();
                opts.ViewLocationFormats.Add("/Notification/Templates/Razor/{0}" + RazorViewEngine.ViewExtension);
            });
            var section = configuration.GetSection(nameof(SendGridOptions));
            var options = section.Get<SendGridOptions>();
            services.Configure<SendGridOptions>(section);
            services.AddTransient<IEmailTemplateRenderer, RazorEmailTemplateRenderer>();
            services.AddTransient<ISendGridClient, SendGridClient>(x => new SendGridClient(options.ApiKey));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            return services;
        }
    }
}
