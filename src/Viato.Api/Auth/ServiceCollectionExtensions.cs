using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Viato.Api.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant>();
            services.AddScoped<FacebookAuthProvider>();
            services.AddScoped<TwitterAuthProvider>();
            services.AddScoped<GoogleAuthProvider>();
            return services;
        }
    }
}
