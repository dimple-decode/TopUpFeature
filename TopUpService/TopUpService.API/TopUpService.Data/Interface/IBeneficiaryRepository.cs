using TopUpService.Database.Entities;
using TopUpService.DTO.Request;

namespace TopUpService.Data
{
    /// <summary>
    /// Interface for Beneficiary Request
    /// </summary>
    public interface IBeneficiaryRepository
    {
        Task AddBeneficiaryAsync(AddBeneficiaryRequest request);
        Task<List<Beneficiary>> GetBeneficiaryByUserIdAsync(int userId);
    }
}
