using Viato.Api.Entities;

namespace Viato.Api.Stores
{
    public interface IExternalAuthProvidersStore
    {
        ExternalAuthProver Get(ExternalProviderType providerType);
    }
}
