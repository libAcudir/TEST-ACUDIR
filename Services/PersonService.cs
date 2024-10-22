using AutoMapper;
using Domain.Entities;
using Repository.Interfaces;
using Services.DTOs;
using Services.Interfaces;

namespace Services
{
    public class PersonService : IPersonService
    {

        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper) 
        {
           _personRepository = personRepository;
           _mapper = mapper;
        }


        public async Task<List<PersonDTO>> GetAll(FilterQueryDTO filterQuery)
        {
            List<Person> persons = await _personRepository.GetAll();
            List<PersonDTO> personsDTO = _mapper.Map<List<PersonDTO>>(persons);

            if (filterQuery == null) return personsDTO;

            var query = personsDTO.Where(p =>
                (string.IsNullOrEmpty(filterQuery.Name) || p.name.ToLower().Contains(filterQuery.Name.ToLower())) &&
                (filterQuery.Age <= 0 || p.age == filterQuery.Age) &&
                (string.IsNullOrEmpty(filterQuery.Address) || p.address.ToLower().Contains(filterQuery.Address.ToLower())) &&
                (string.IsNullOrEmpty(filterQuery.PhoneNumber) || p.phoneNumber.Contains(filterQuery.PhoneNumber)) &&
                (string.IsNullOrEmpty(filterQuery.Profession) || p.profession.ToLower().Contains(filterQuery.Profession.ToLower()))
            );



            return query.ToList();
        }


        public async Task<List<PersonDTO>> AddPerson(List<PersonDTO> personsToAddDTO)
        {
            List<Person> personsToAdd = _mapper.Map<List<Person>>(personsToAddDTO);

            List<Person> persons =  await _personRepository.AddPerson(personsToAdd);

            return _mapper.Map<List<PersonDTO>>(persons);
        }

        public async Task<List<PersonDTO>> UpdatePerson(string namePerson, PersonDTO personsToUpdateDTO)
        {
            List<Person> persons = await _personRepository.GetAll();

            Person personToUpdate = persons.FirstOrDefault(p => p.NombreCompleto.ToLower() == namePerson.ToLower());



            if (personToUpdate != null)
            {

                UpdateIfNotNull(personsToUpdateDTO.name, value => personToUpdate.NombreCompleto = value);
                UpdateIfNotNull(personsToUpdateDTO.age, value => personToUpdate.Edad = value);
                UpdateIfNotNull(personsToUpdateDTO.address, value => personToUpdate.Domicilio = value);
                UpdateIfNotNull(personsToUpdateDTO.phoneNumber, value => personToUpdate.Telefono = value);
                UpdateIfNotNull(personsToUpdateDTO.profession, value => personToUpdate.Profesion = value);


                await _personRepository.UpdateDataPerson(persons);

                List<PersonDTO> updatedList = _mapper.Map<List<PersonDTO>>(persons);
                return updatedList;
            }

            throw new KeyNotFoundException($"Person with name: {namePerson} not found");
        }
        private void UpdateIfNotNull(string? newValue, Action<string> updateAction)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                updateAction(newValue);
            }
        }

        private void UpdateIfNotNull(int? newValue, Action<int> updateAction)
        {
            if (newValue.HasValue)
            {
                updateAction(newValue.Value);
            }
        }
    }
}
