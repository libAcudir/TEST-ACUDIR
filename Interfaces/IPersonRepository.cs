using Domain.Command;
using Domain.Dto;
using Repository.Results;

namespace Interfaces
{
    public interface IPersonRepository
    {
        public Task<ResultApp> PostAsync(PersonCreateCommand commandcreate);
        public Task<ResultApp> PutAsync(PersonUpdateCommand commandUpdate);
        public Task<List<PersonDto>> GetAllAsync(string? NombreCompleto, string? Edad, string? Domicilio, string? Profesion);
    }
}