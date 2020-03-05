using System;
using System.Collections.Generic;

namespace Viato.Api.Entities
{
    public enum ContributionPipelineStatus
    {
        Inactive,
        Active,
        Suspended
    }

    [Flags]
    public enum ContributionPipelineTypes
    {
        NoLimit,
        LimitByAmount,
        LimitByDate
    }

    public class ContributionPipeline
    {
        public long Id { get; set; }
        public ContributionPipelineStatus Status { get; set; } = ContributionPipelineStatus.Inactive;
        public ContributionPipelineTypes Types { get; set; } = ContributionPipelineTypes.NoLimit;
        public long SourceOrgId { get; set; }
        public long DestinationOrgId { get; set; }
        public string ContributionCurrency { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal AmountLimit { get; set; }
        public DateTimeOffset? DateLimit { get; set; }
        public string OwnerPublicKey { get; set; }
        public string OwnerPrivateKey { get; set; } // TODO to be removed later, we will just store public key
        public virtual AppUser SourceOrg { get; set; }
        public virtual AppUser DestinationOrg { get; set; }
        public virtual List<Contribution> Contributions { get; set; } = new List<Contribution>();
    }
}
