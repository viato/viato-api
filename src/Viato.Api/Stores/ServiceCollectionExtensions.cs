using Microsoft.Extensions.DependencyInjection;

namespace Viato.Api.Stores
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            services.AddTransient<IExternalAuthProvidersStore, InMemmoryExternalAuthProvidersStore>();
            return services;
        }
    }
}
