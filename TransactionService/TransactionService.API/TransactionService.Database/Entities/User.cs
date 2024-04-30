using TransactionService.Database.Entities;

namespace TransactionService.Database
{
    /// <summary>
    /// User Class
    /// </summary>
    public class User : BaseEntity
    {
        public decimal Balance { get; set; }
    }
}
