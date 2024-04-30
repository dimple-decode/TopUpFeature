using System.Linq.Expressions;

namespace TopUpService.Data
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<List<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> exp);
    }
}
