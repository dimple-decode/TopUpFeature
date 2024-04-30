using TransactionService.DTO;
using TransactionService.DTO.Request;

namespace TransactionService.Logic
{
    /// <summary>
    /// User Transaction Service Interface
    /// </summary>
    public interface IUserTransactionService
    {
        public Task<decimal?> GetUserBalanceById(int userId);
        public Task<Tuple<bool, string>> DebitCreditTransaction(UserTransactionRequest userTransaction);
    }
}
