using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class PostModel
    {
        public long Id { get; set; }

        public PostStatus Status { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string ImageBlobUri { get; set; }

        public long AuthorOrganizationId { get; set; }

        public long ContributionPipelineId { get; set; }
    }
}
