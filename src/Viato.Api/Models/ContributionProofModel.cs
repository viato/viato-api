using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class ContributionProofModel
    {
        public ContributionProofStatus ProofStatus { get; set; }

        public string ProofNetwork { get; set; }

        public string BlockchainTransactionId { get; set; }
    }
}
