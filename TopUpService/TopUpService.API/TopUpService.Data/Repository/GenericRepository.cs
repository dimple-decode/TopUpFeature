using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TopUpService.Database;

namespace TopUpService.Data
{
    /// <summary>
    /// Generic Repository Implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> exp)
        {
            return await _dbContext.Set<T>().Where(exp).ToListAsync();
        }

        public async Task<T?> GetEntityAsync(Expression<Func<T, bool>> exp)
        {
            return await _dbContext.Set<T>().Where(exp).FirstOrDefaultAsync();
        }
    }
}
