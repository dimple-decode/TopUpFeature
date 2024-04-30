using TopUpService.Database.Entities;
using TopUpService.DTO.Request;
using TopUpService.DTO.Response;

namespace TopUpService.Logic
{
    /// <summary>
    /// Interface For Top Up Service
    /// </summary>
    public interface IUserTopUpService
    {
        Task<Tuple<bool, string>> AddBeneficiaryAsync(AddBeneficiaryRequest request);
        Task<List<BeneficiaryDetails>> GetBeneficiariesByUserIdAsync(int userId);
        Task<List<string>> GetTopUpOptionsAsync();
        Task<Tuple<bool, string>> TopUp(TopUpRequest request);
    }
}
