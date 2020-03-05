namespace Viato.Api.Entities
{
    public enum ContributionProofStatus
    {
        NotPublished,
        Published,
        Succeeded
    }

    public class ContributionProof
    {
        public long Id { get; set; }
        public ContributionProofStatus Status { get; set; }
        public string Network { get; set; }
        public string BlockchainTransactionId { get; set; }
    }
}
