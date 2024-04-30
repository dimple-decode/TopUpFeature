using TransactionService.Database;

namespace TransactionService.Data
{
    /// <summary>
    /// User Repository Class
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// User Repository Constructor
        /// </summary>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;

            
            if(_context.Users.Count() <=0)
            {
                var users = new List<User>();

                //Initialize with Mock User Data
                users.Add(new User { Id = 1, Balance = 10000m });
                users.Add(new User { Id = 2, Balance = 20000m });

                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
           
        }

        /// <summary>
        /// Get User Details By User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetById(int id)
        {
            var users = await Task.Run(() => _context.Users.FirstOrDefault(u => u.Id == id));
            return users;
        }

        /// <summary>
        /// Get User Balance By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<decimal?> GetBalanceById(int id)
        {
            var user = await GetById(id);
            if (user == null) return null;
            return user.Balance;
        }

        /// <summary>
        /// Update User Balance with update amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedAmount"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBalance(int id, decimal updatedAmount)
        {
            var user = await GetById(id);
            if (user == null) return false;
            user.Balance = updatedAmount;
           _context.Users.Update(user);
            bool success = await _context.SaveChangesAsync() > 0 ? true: false;
            return success;
        }
    }
}
