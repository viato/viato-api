using System;

namespace Viato.Api.Entities
{
    [Flags]
    public enum ContributionPipelineTypes
    {
        NoLimit,
        LimitByAmount,
        LimitByDate,
    }
}
