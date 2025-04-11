using Business.Abstraction.Exceptions;
using Business.Extensions;
using FluentValidation.Results;
using AutoMapperProfile = AutoMapper.Profile;

namespace Business.AutoMapper
{
    public partial class MappingProfile : AutoMapperProfile
    {
        public MappingProfile()
        {
            CreateGenericMappings();
            CreateInterventionMappings();
        }

        private void CreateGenericMappings()
        {
            CreateMap<ValidationFailure, BusinessError>()
                .ForMember(dest => dest.Code, act => act.MapFrom(src =>
                    Enum.IsDefined(typeof(BusinessErrorCode), src.ErrorCode)
                        ? Enum.Parse<BusinessErrorCode>(src.ErrorCode)
                        : BusinessErrorCode.InconsistentModel))
                .ForMember(dest => dest.Message, act => act.MapFrom(src => src.ErrorMessage))
                .ForMember(dest => dest.ValueInFailure, act => act.MapFrom(src => src.AttemptedValue));
        }
    }
}
