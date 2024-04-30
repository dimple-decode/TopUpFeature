using TopUpService.Database;
using TopUpService.Database.Entities;
using TopUpService.DTO.Request;

namespace TopUpService.Data
{
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context):base(context)
        {
            _context = context;

            if (_context.Users.Count() <= 0)
            {
                var users = new List<User>();

                //Initialize with Mock User Data
                users.Add(new User { Id = 1, IsVerified = true });
                users.Add(new User { Id = 2, IsVerified = false});

                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await GetById(id);
        }
    }
}
