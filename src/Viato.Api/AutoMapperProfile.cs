using AutoMapper;
using Viato.Api.Entities;
using Viato.Api.Models;

namespace Viato.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Organization, OrganizationModel>();
        }
    }
}
