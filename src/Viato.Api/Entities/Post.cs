namespace Viato.Api.Entities
{
    public class Post : EntityBase<long>
    {
        public long ContributionPipelineId { get; set; }

        public long AuthorOrganizationId { get; set; }

        public PostStatus Status { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string ImageBlobUri { get; set; }

        public virtual Organization AuthorOrganization { get; set; }

        public virtual ContributionPipeline ContributionPipeline { get; set; }
    }
}
