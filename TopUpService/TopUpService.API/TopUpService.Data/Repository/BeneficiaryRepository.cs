using TopUpService.Database;
using TopUpService.Database.Entities;
using TopUpService.DTO.Request;

namespace TopUpService.Data
{
    /// <summary>
    /// Beneficiary Repository Implementation
    /// </summary>
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(ApplicationDbContext context): base(context)
        {
        }

        /// <summary>
        /// Add Beneficiary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddBeneficiaryAsync(AddBeneficiaryRequest request)
        {
            var beneficiary = new Beneficiary()
            {
                UserId = request.UserId,
                IsActive = true,
                NickName = request.NickName,
                PhoneNumber = request.PhoneNumber
            };

           await Add(beneficiary);
        }

        /// <summary>
        /// Get Beneficiaries By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Beneficiary>> GetBeneficiaryByUserIdAsync(int userId)
        {
            var beneficiaries = await GetListAsync(x=>x.UserId.Equals(userId) && x.IsActive);
            return beneficiaries;
        }
    }
}
