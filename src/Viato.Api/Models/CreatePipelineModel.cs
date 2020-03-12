using System;
using System.ComponentModel.DataAnnotations;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class CreatePipelineModel
    {
        public ContributionPipelineStatus Status { get; set; } = ContributionPipelineStatus.Inactive;

        public ContributionPipelineTypes Types { get; set; } = ContributionPipelineTypes.NoLimit;

        [Required]
        public string DisplayName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public long SourceOrganizationId { get; set; }

        public long DestinationOrganizationId { get; set; }

        [Required]
        public string ContributionCurrency { get; set; }

        public decimal? AmountLimit { get; set; }

        public DateTimeOffset? DateLimit { get; set; }
    }
}
