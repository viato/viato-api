using System.ComponentModel.DataAnnotations;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class CreateOrUpdatePostModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Body { get; set; }

        [Required]
        public PostStatus Status { get; set; }

        public long? ContributionPipelineId { get; set; }
    }
}
