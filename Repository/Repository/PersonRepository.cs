
using AutoMapper;
using Domain.Command;
using Domain.Dto;
using Domain.Entities;
using Interfaces;
using Repository.Results;
using Repository.Utils;
using System.Text.Json;

namespace Repository.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMapper mapper;
        private IGenericRepository repository;
        private readonly string filePath = "Test.json";

        public PersonRepository(IMapper _mapper, IGenericRepository repository_)
        {
            mapper = _mapper;
            repository = repository_;
        }
        public Task<List<PersonDto>> GetAllAsync(string? NombreCompleto, string? Edad, string? Domicilio, string? Profesion)
        {
            string filePath = "Test.json";

            string jsonString = File.ReadAllText(filePath);
            List<Person> personas = JsonSerializer.Deserialize<List<Person>>(jsonString);

            var res = personas
                .Where(x => (NombreCompleto == null || !NombreCompleto.Any() || NombreCompleto.Contains(x.NombreCompleto))
                                && (Edad == null || !Edad.Any() || Edad.Contains(x.Edad))
                                && (Domicilio == null || !Domicilio.Any() || Domicilio.Contains(x.Domicilio))
                                && (Profesion == null || !Profesion.Any() || Profesion.Contains(x.Profesion))
                                  )
                  .OrderByDescending(x => x.NombreCompleto).ToList<Person>();

            var map = mapper.Map<List<PersonDto>>(res);
            return Task.FromResult(map);
        }

        public Task<ResultApp> PostAsync(PersonCreateCommand commandCreate)
        {
            ResultApp res = new ResultApp();

            List<Person> personas = LoadData();
            personas.Add(MapToCreateModel(commandCreate));

            repository.Write( personas);
            res.Succeeded = true;
            return Task.FromResult(res);
        }

        public Person MapToCreateModel(PersonCreateCommand commandCreate)
        {
            Person person = new Person()
            {
                PersonId = GenerateRandomText.GenerateRandom(),
                NombreCompleto = commandCreate.NombreCompleto,
                Domicilio = commandCreate.Domicilio,
                Edad = commandCreate.Edad,
                Telefono = commandCreate.Telefono,
                Profesion = commandCreate.Profesion
            };
            return person;
        }

        public Task<ResultApp> PutAsync(PersonUpdateCommand commandUpdate)
        {
            ResultApp res = new ResultApp();

            List<Person> personas = LoadData();
            var update = personas.Find(x => x.PersonId.Equals(commandUpdate.PersonId));

            Person person = new Person()
            {
                PersonId = update.PersonId,
                NombreCompleto = commandUpdate.NombreCompleto ?? update.NombreCompleto,
                Domicilio = commandUpdate.Domicilio ?? update.Domicilio,
                Edad = commandUpdate.Edad ?? update.Edad,
                Telefono = commandUpdate.Telefono ?? update.Telefono,
                Profesion = commandUpdate.Profesion ?? update.Profesion
            };

            if (update == null)
            {
                res.Succeeded = false;
                res.message = "La persona no se encuentra en los registros";
                return Task.FromResult(res);
            }
            int indice = personas.IndexOf(update);
            if (indice != -1)
            {
                personas[indice] = person;
            }
            repository.Write(personas);
            res.Succeeded = true;
            return Task.FromResult(res);

        }

        public List<Person> LoadData()
        {
            string jsonString = File.ReadAllText(filePath);
            List<Person> personas = JsonSerializer.Deserialize<List<Person>>(jsonString);
            return personas;
        }

    }
}