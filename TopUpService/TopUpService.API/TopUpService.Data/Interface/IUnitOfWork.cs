namespace TopUpService.Data
{
    /// <summary>
    /// Interface for Unit Of Work
    /// </summary>
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<bool> Complete();
    }
}
