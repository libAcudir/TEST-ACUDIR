namespace Interfaces
{
    public interface IGenericRepository
    {
        public Task Write<T>(List<T> entities) where T : class;

    }
}