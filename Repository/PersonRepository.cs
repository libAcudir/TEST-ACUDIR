using Domain.Entities;
using Repository.Interfaces;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _dataTest = @"Test.json"; 


        public async Task<List<Person>> GetAll()
        {
            string jsonData = await File.ReadAllTextAsync(_dataTest); 
            List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(jsonData);
            return persons;
        }

        public async Task<List<Person>> AddPerson(List<Person> personsToAdd)
        {

        
            List<Person> listPersons = ReadPersonsFromFile(await File.ReadAllTextAsync(_dataTest));


            listPersons.AddRange(personsToAdd);

        
            var dataJson = JsonConvert.SerializeObject(listPersons, Formatting.Indented);

            await File.WriteAllTextAsync(_dataTest, dataJson);

            return listPersons;
        }

        public async Task<List<Person>> UpdateDataPerson(List<Person> persons) 
        {


            string updatedData = JsonConvert.SerializeObject(persons, Formatting.Indented);
            await File.WriteAllTextAsync(_dataTest, updatedData);

            List<Person> listPersons = ReadPersonsFromFile(await File.ReadAllTextAsync(_dataTest));


            return listPersons;

        }

        private List<Person> ReadPersonsFromFile(string fileContent)
        {
        
            List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(fileContent);
            return persons;
        }
    }
}
