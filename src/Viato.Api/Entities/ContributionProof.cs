using System.Collections.Generic;

namespace Viato.Api.Entities
{
    public enum ContributionProofStatus
    {
        NotPublished,
        Published,
        Succeeded,
    }

    public class ContributionProof : EntityBase
    {
        public ContributionProofStatus Status { get; set; }

        public string Network { get; set; }

        public string BlockchainTransactionId { get; set; }

        public virtual List<Contribution> Contributions { get; set; } = new List<Contribution>();
    }
}
