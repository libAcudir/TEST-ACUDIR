using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IPersonRepository
    {
        public Task<List<Person>> GetAll();
        public Task<List<Person>> AddPerson(List<Person> persons);
        public Task<List<Person>> UpdateDataPerson(List<Person> persons);
    }
}
