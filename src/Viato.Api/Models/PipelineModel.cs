using System;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class PipelineModel
    {
        public long Id { get; set; }

        public ContributionPipelineStatus Status { get; set; } = ContributionPipelineStatus.Inactive;

        public ContributionPipelineTypes Types { get; set; } = ContributionPipelineTypes.NoLimit;

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public long SourceOrganizationId { get; set; }

        public long DestinationOrganizationId { get; set; }

        public string ContributionCurrency { get; set; }

        public decimal CollectedAmount { get; set; }

        public decimal AmountLimit { get; set; }

        public DateTimeOffset? DateLimit { get; set; }
    }
}
