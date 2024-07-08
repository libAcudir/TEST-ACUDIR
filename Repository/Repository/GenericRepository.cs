
using Interfaces;
using System.Linq;
using System.Text.Json;

namespace Repository.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly string filePath = "Test.json";

        public Task Write<T>(List<T> entities) where T : class
        {
            string updatedJsonString = JsonSerializer.Serialize(entities);
            File.WriteAllText(filePath, updatedJsonString);
            return Task.CompletedTask;
        }
    }
}