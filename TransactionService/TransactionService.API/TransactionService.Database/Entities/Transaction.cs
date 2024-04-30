using TransactionService.Database.Entities;

namespace TransactionService.Database
{
    /// <summary>
    /// Transaction class
    /// </summary>
    public class Transaction : BaseEntity
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
