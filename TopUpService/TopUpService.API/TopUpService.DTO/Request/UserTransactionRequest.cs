using TopUpService.Common;

namespace TopUpService.DTO
{
    public class UserTransactionRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}