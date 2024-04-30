using TopUpService.DTO;
using TopUpService.DTO.Response;

namespace TopUpService.Logic
{
    public interface IUserTransactionHttpService
    {
        Task<UserTransactionResponse> GetUserBalanceAsync(int userId);
        Task<UserTransactionResponse> DebitCreditTransaction(UserTransactionRequest request);
    }
}
