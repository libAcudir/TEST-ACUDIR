using Acudir.Test.Apis.Extra.Exceptions;
using Acudir.Test.Apis.Interfaces.IRepositorys;
using Acudir.Test.Apis.Models.CustomEntities;
using Acudir.Test.Apis.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Acudir.Test.Apis.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly List<Persona> _personas;
        private readonly DatabaseJsonOption _databaseJsonOption;
        private readonly string internalError = "Hubo un error interno";

        public PersonaRepository(List<Persona> personas, IOptions<DatabaseJsonOption> options)
        {
            _personas = personas;
            _databaseJsonOption = options.Value;
        }

        public IEnumerable<Persona> GetAll()
        {
            try
            {
                return _personas;
            }
            catch (Exception)
            {
                throw new DatabaseException(internalError);
            }

        }

        public Persona GetById(int id)
        {
            return _personas.FirstOrDefault(p => p.Id == id);
        }

        public void AddRange(IEnumerable<Persona> personaDB_List)
        {
            try
            {
                _personas.AddRange(personaDB_List);
                SaveToFile();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(internalError);
            }

        }

        public void Update(Persona updatedPersona)
        {
            try
            {
                SaveToFile();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(internalError);
            }

        }

        public void Delete(Persona existingPersona)
        {
            try
            {
                _personas.Remove(existingPersona);
                SaveToFile();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(internalError);
            }
            
        }

        private void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(_personas, Formatting.Indented);

            string fileName = _databaseJsonOption.FileName;

            File.WriteAllText(fileName, json);
        }

    }
}
