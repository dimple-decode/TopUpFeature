using TopUpService.Database;
using TopUpService.Database.Entities;

namespace TopUpService.Data.Repository
{
    /// <summary>
    /// Transaction Reporitory
    /// </summary>
    public class TransactionRepository: GenericRepository<TopUpTransaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context): base(context)
        {

        }

        /// <summary>
        /// Add TopUp Transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task TopUp(TopUpTransaction transaction) 
        { 
            transaction.CreatedOn = DateTime.Now;   
           await Add(transaction);
        }

        /// <summary>
        /// Get Beneficiary TopUp Transactions
        /// </summary>
        /// <param name="beneficiaryId"></param>
        /// <returns></returns>
        public async Task<List<TopUpTransaction>> GetTransactionsByBeneficiaryId(int beneficiaryId)
        {
            return await GetListAsync(x => x.BeneficiaryId.Equals(beneficiaryId));
        }

        /// <summary>
        /// Get All Beneficiary TopUp Transactions By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<TopUpTransaction>> GetTransactionsByUserId(int userId)
        {
            return await GetListAsync(x => x.UserId.Equals(userId));
        }
    }
}
