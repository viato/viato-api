using System;
using System.Collections.Generic;

namespace Viato.Api.Entities
{
    public class ContributionPipeline : EntityBase<long>
    {
        public ContributionPipelineStatus Status { get; set; } = ContributionPipelineStatus.Inactive;

        public ContributionPipelineTypes Types { get; set; } = ContributionPipelineTypes.NoLimit;

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public long SourceOrganizationId { get; set; }

        public long DestinationOrganizationId { get; set; }

        public string ContributionCurrency { get; set; }

        public decimal CollectedAmount { get; set; }

        public decimal AmountLimit { get; set; }

        public DateTimeOffset? DateLimit { get; set; }

        public string OwnerPublicKey { get; set; }

        public string OwnerPrivateKey { get; set; } // TODO to be removed later, we will just store public key

        public virtual Organization SourceOrganizaton { get; set; }

        public virtual Organization DestinationOrganization { get; set; }

        public virtual List<Contribution> Contributions { get; set; } = new List<Contribution>();

        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
