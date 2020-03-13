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
            CreateMap<Post, PostModel>();

            // TODO: write custom mapping, for private contrs and joining contribution proofs
            CreateMap<Contribution, ContributionModel>();
        }
    }
}
