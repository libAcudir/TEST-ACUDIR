using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPersonService
    {
        public Task<List<PersonDTO>> GetAll(FilterQueryDTO filterQuery);
        public Task <List<PersonDTO>> AddPerson(List<PersonDTO> personDTOs);
        public Task<List<PersonDTO>> UpdatePerson(string namePerson, PersonDTO personsToUpdateDTO);

    }
}
