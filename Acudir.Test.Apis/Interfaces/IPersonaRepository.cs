using Acudir.Test.Apis.Domain;
using Acudir.Test.Apis.DTOs;
using Acudir.Test.Apis.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acudir.Test.Apis.Interfaces
{
    public interface IPersonaRepository
    {
        Task<List<PersonaDTO>> GetAllPersonas(PersonaFilters filters);
        Task<List<PersonaDTO>> AddPersonas(List<PersonaForAddDTO> personasForAdd);
        Task<PersonaDTO> EditPersona(string nombreCompleto, PersonaForEditDTO editPersona);
        Task<PersonaDTO> DeletePersona(string nombreCompleto);
    }
}
