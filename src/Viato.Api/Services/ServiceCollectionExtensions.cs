using Microsoft.Extensions.DependencyInjection;

namespace Viato.Api.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDnsProofService, DnsProofService>();
            return services;
        }
    }
}
