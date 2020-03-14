using System;

namespace Viato.Api.Entities
{
    public class StagedContribution : EntityBase<Guid>
    {
        public long ContributionId { get; set; }
    }
}
