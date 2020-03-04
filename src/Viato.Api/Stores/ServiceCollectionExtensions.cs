using Microsoft.Extensions.DependencyInjection;
using Viato.Api.Stores;

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
