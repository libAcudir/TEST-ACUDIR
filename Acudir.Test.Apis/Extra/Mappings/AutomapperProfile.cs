using Acudir.Test.Apis.Dominio.DTOs;
using Acudir.Test.Apis.Models.DTOs;
using Acudir.Test.Apis.Models.Entities;
using AutoMapper;

namespace Acudir.Test.Apis.Extra.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Persona, PersonaDTO>().ReverseMap();
            CreateMap<PersonaDTO_Post, Persona>();
        }
    }
}
