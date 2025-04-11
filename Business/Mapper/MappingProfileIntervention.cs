using AutoMapper;
using Business.Abstraction.Models;
using DataAccess.Abstraction.Entities;
using DataAccess.Abstraction.Entity;


namespace Business.AutoMapper
{
    public partial class MappingProfile : Profile
    {
        private void CreateInterventionMappings()
        {

            CreateMap<InterventionEntity, InterventionModel>().ReverseMap();
            CreateMap<InterventionEntity, GetInterventionModel>()
                .ForMember(dest => dest.Technicians, opt => opt.MapFrom(src => src.Technician))
                .ReverseMap()
                .ForMember(dest => dest.Technician, opt => opt.MapFrom(src => src.Technicians));

            CreateMap<UserEntity, UserModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
        }
    }
}
