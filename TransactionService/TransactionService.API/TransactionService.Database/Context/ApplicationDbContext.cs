using Microsoft.EntityFrameworkCore;

namespace TransactionService.Database
{
    /// <summary>
    /// Application DBContext
    /// </summary>
   public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

         public DbSet<User> Users { get; set; }
         public DbSet<Transaction> Transactions { get; set; }
    }

}
