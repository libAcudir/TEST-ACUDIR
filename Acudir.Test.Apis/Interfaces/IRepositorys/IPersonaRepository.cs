using Acudir.Test.Apis.Models.Entities;

namespace Acudir.Test.Apis.Interfaces.IRepositorys
{
    public interface IPersonaRepository
    {
        IEnumerable<Persona> GetAll();
        Persona GetById(int id);
        void AddRange(IEnumerable<Persona> personaDB_List);
        void Update(Persona updatedPersona);
        void Delete(Persona existingPersona);
    }
}
