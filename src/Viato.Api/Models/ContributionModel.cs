namespace Viato.Api.Models
{
    public class ContributionModel
    {
        public long ContributionPipelineId { get; set; }

        public long? ContributorId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string TorTokenId { get; set; }

        public ContributionProofModel Proof { get; set; }
    }
}
