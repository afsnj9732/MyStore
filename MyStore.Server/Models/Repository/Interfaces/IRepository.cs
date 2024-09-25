namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        void Add(T entity);
        IEnumerable<T> GetAll();
    }
}
