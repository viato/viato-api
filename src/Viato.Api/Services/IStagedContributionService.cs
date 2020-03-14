using System;
using System.Threading.Tasks;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public interface IStagedContributionService
    {
        Task AttachStagedContributionAsync(Guid stagedContributionId, AppUser appUser);
    }
}
