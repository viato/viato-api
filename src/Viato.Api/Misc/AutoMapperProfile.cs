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

            CreateMap<ContributionProof, ContributionProofModel>();
            CreateMap<Contribution, ContributionModel>()
                .ForMember(m => m.ContributorId, opt => opt.MapFrom(src => src.IsPrivate ? null : src.ContributorId));
        }
    }
}
