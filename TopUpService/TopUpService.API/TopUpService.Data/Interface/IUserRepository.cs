using TopUpService.Database.Entities;

namespace TopUpService.Data
{
    /// <summary>
    /// Interface for User Repository 
    /// </summary>
    public interface IUserRepository
    {
        public Task<User?> GetUserByIdAsync(int id);
    }
}
