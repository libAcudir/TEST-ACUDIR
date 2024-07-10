using Acudir.Test.Apis.Domain;
using Acudir.Test.Apis.DTOs;
using Acudir.Test.Apis.Helpers;
using Acudir.Test.Apis.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Acudir.Test.Apis.Repository
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly string _jsonFilePath = "Test.json";
        private readonly IMapper _mapper;

        public PersonaRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        private async Task<List<Persona>> ReadPersonasFromFile()
        {
            try
            {
                var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
                return JsonConvert.DeserializeObject<List<Persona>>(jsonData) ?? new List<Persona>();
            }
            catch (IOException)
            {
                return new List<Persona>();
            }
        }

        private async Task WritePersonasToFile(List<Persona> personas)
        {
            var jsonData = JsonConvert.SerializeObject(personas, Formatting.Indented);
            await File.WriteAllTextAsync(_jsonFilePath, jsonData);
        }

        public async Task<List<PersonaDTO>> GetAllPersonas(PersonaFilters filters)
        {
            var personas = await ReadPersonasFromFile();

            if (filters != null)
            {
                personas = personas.Where(p =>
                    (string.IsNullOrEmpty(filters.Nombre) || p.NombreCompleto.ToLower().Contains(filters.Nombre.ToLower())) &&
                    (string.IsNullOrEmpty(filters.Edad) || p.Edad.ToLower() == filters.Edad.ToLower()) &&
                    (string.IsNullOrEmpty(filters.Domicilio) || p.Domicilio.ToLower().Contains(filters.Domicilio.ToLower())) &&
                    (string.IsNullOrEmpty(filters.Telefono) || p.Telefono.ToLower().Contains(filters.Telefono.ToLower())) &&
                    (string.IsNullOrEmpty(filters.Profesion) || p.Profesion.ToLower().Contains(filters.Profesion.ToLower()))
                ).ToList();
            }

            return _mapper.Map<List<PersonaDTO>>(personas);
        }

        public async Task<List<PersonaDTO>> AddPersonas(List<PersonaForAddDTO> personasForAdd)
        {
            var personas = _mapper.Map<List<Persona>>(personasForAdd);
            var personasData = await ReadPersonasFromFile();

            personasData.AddRange(personas);
            await WritePersonasToFile(personasData);

            return _mapper.Map<List<PersonaDTO>>(personas);
        }

        public async Task<PersonaDTO> EditPersona(string nombreCompleto, PersonaForEditDTO editPersona)
        {
            var personas = await ReadPersonasFromFile();
            var personaForEdit = personas.FirstOrDefault(p => p.NombreCompleto.ToLower() == nombreCompleto.ToLower());

            if (personaForEdit != null)
            {
                _mapper.Map(editPersona, personaForEdit);
                await WritePersonasToFile(personas);
                return _mapper.Map<PersonaDTO>(personaForEdit);
            }

            return null;
        }

        public async Task<PersonaDTO> DeletePersona(string nombreCompleto)
        {
            var personas = await ReadPersonasFromFile();
            var persona = personas.FirstOrDefault(p => p.NombreCompleto.ToLower() == nombreCompleto.ToLower());

            if (persona != null)
            {
                personas.Remove(persona);
                await WritePersonasToFile(personas);
                return _mapper.Map<PersonaDTO>(persona);
            }

            return null;
        }
    }
}
