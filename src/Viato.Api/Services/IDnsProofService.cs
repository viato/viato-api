using System.Threading.Tasks;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public interface IDnsProofService
    {
        Task<bool> VerifyProofAsync(Organization organization);
    }
}
