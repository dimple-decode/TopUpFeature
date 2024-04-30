using TopUpService.Database.Entities;

namespace TopUpService.Data
{
    public interface ITransactionRepository: IGenericRepository<TopUpTransaction>
    {
        public Task TopUp(TopUpTransaction transaction);
        public Task<List<TopUpTransaction>> GetTransactionsByBeneficiaryId(int beneficiaryId);
        public Task<List<TopUpTransaction>> GetTransactionsByUserId(int userId);
    }
}
