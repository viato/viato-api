using Viato.Api.Entities;

namespace Viato.Api.Stores
{
    public interface IExternalAuthProvidersStore
    {
        ExternalAuthProvider Get(ExternalProviderType providerType);
    }
}
