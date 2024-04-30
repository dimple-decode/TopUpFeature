using TransactionService.Common;

namespace TransactionService.DTO.Request
{
    /// <summary>
    /// DTO For User Transaction
    /// </summary>
    public class UserTransactionRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
