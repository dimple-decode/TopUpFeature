using TransactionService.Database;

namespace TransactionService.Data
{
    /// <summary>
    /// Interface for User Repository 
    /// </summary>
    public interface IUserRepository
    {
        public Task<User?> GetById(int id);
        public Task<decimal?> GetBalanceById(int id);
        public Task<bool> UpdateBalance(int id, decimal amount);
    }
}
