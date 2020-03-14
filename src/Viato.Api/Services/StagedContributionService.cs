using System;
using System.Threading.Tasks;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public class StagedContributionService : IStagedContributionService
    {
        private readonly ViatoContext _dbContext;

        public StagedContributionService(ViatoContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AttachStagedContributionAsync(Guid stagedContributionId, AppUser user)
        {
            var stagedContribution = await _dbContext.StagedContributions.FindAsync(stagedContributionId);
            if (stagedContribution != null)
            {
                var contribution = await _dbContext.Contributions.FindAsync(stagedContribution.ContributionId);
                if (contribution != null && contribution.ContributorId == null)
                {
                    contribution.ContributorId = user.Id;
                    contribution.IsPrivate = false;

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
