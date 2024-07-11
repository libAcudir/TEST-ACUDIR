using AutoMapper;
using Acudir.Test.Apis.Domain;
using Acudir.Test.Apis.DTOs;

namespace Acudir.Test.Apis.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Persona, PersonaDTO>();
            CreateMap<PersonaDTO, Persona>();

            CreateMap<PersonaForAddDTO, Persona>();
            CreateMap<PersonaForEditDTO, Persona>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
