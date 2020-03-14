using System.Collections.Generic;

namespace Viato.Api.Entities
{
    public class ContributionProof : EntityBase<long>
    {
        public ContributionProofStatus Status { get; set; }

        public string Network { get; set; }

        public string BlockchainTransactionId { get; set; }

        public virtual List<Contribution> Contributions { get; set; } = new List<Contribution>();
    }
}
