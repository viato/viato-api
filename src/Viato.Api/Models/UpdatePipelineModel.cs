using System;
using System.ComponentModel.DataAnnotations;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class UpdatePipelineModel
    {
        public ContributionPipelineStatus? Status { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public decimal? AmountLimit { get; set; }

        public DateTimeOffset? DateLimit { get; set; }
    }
}
