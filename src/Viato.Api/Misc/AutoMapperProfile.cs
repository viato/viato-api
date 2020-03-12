using AutoMapper;
using Viato.Api.Entities;
using Viato.Api.Models;

namespace Viato.Api.Misc
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Organization, OrganizationModel>();
            CreateMap<ContributionPipeline, PipelineModel>();
        }
    }
}
