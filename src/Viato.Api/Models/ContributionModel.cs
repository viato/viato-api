using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class ContributionModel
    {
        public long ContributionPipelineId { get; set; }

        public long? ContributorId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string TorTokenId { get; set; }

        public ContributionProofStatus ProofStatus { get; set; }

        public string ProofNetwork { get; set; }

        public string BlockchainTransactionId { get; set; }
    }
}
