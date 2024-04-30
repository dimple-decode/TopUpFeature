using Microsoft.Extensions.Logging;
using TransactionService.Common;
using TransactionService.Data;
using TransactionService.DTO.Request;

namespace TransactionService.Logic
{
    /// <summary>
    /// User Trasaction Service
    /// </summary>
    public class UserTransactionService : IUserTransactionService
    {
        private readonly IUserRepository _userRepository;

        public UserTransactionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Credit Amount
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amountToCredit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Tuple<bool, string>> DebitCreditTransaction(UserTransactionRequest userTransaction)
        {
            string message = string.Empty;

            var user = await _userRepository.GetById(userTransaction.UserId);
            if (user == null)
            {
                return new Tuple<bool, string>(false, "User not found");
            }

            if (userTransaction.TransactionType.Equals(TransactionType.CREDIT))   //Credit
            {
                user.Balance += userTransaction.Amount;
                message = "Amount Credited Successfully";
            }
            else if (userTransaction.TransactionType.Equals(TransactionType.DEBIT))   //Debit
            {
                if (user.Balance < userTransaction.Amount) return new Tuple<bool, string>(false, "Insifficient Balance");

                user.Balance -= userTransaction.Amount;
                message = "Amount Debited Successfully";
            }

            bool isSuccess = await _userRepository.UpdateBalance(user.Id, user.Balance);

            return new Tuple<bool, string>(isSuccess, message);
        }

     
        /// <summary>
        /// Get User Balance By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<decimal?> GetUserBalanceById(int userId)
        {
            //Get User Balance
            var balance = await _userRepository.GetBalanceById(userId);

            //If null then user doesn't exist 
            if (balance == null) return null;

            return balance;
        }
    }
}
