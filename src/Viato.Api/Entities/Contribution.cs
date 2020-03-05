namespace Viato.Api.Entities
{
    public class Contribution
    {
        public long Id { get; set; }

        public long ContributionPipelineId { get; set; }

        public long ContributorId { get; set; }

        public decimal Amount { get; set; }

        public string TorTokenId { get; set; }

        public string TorToken { get; set; }

        public long? ContributionProofId { get; set; }

        public bool IsPrivate { get; set; }

        public virtual AppUser Contributor { get; set; }

        public virtual ContributionProof ContributionProof { get; set; }

        public virtual ContributionPipeline ContributionPipeline { get; set; }
    }
}
