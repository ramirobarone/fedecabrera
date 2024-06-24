using Entities;

namespace Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        Task<T> Create(string description);
        Task<bool> Delete(int id);
        Task<T?> Put(int id, string description);
        Task<T?> GetId(int id);
    }
}
