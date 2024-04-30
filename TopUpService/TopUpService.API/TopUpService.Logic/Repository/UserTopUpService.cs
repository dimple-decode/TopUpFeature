using AutoMapper;
using TopUpService.Common;
using TopUpService.Data;
using TopUpService.Database.Entities;
using TopUpService.DTO;
using TopUpService.DTO.Request;
using TopUpService.DTO.Response;

namespace TopUpService.Logic
{
    /// <summary>
    /// Top Up Service Implementation
    /// </summary>
    public class UserTopUpService : IUserTopUpService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTransactionHttpService _userTransactionHttpService;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public UserTopUpService(IUserRepository userRepository, IUserTransactionHttpService userTransactionHttpService, IBeneficiaryRepository beneficiaryRepository,
            IUnitOfWork unitOfWork, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userTransactionHttpService = userTransactionHttpService;
            _beneficiaryRepository = beneficiaryRepository;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        #region "Public Methods"

        /// <summary>
        /// Add Beneficiary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> AddBeneficiaryAsync(AddBeneficiaryRequest request)
        {
            var beneficiaries = await _beneficiaryRepository.GetBeneficiaryByUserIdAsync(request.UserId);

            //Check if the user is null or have more than 5 beneficiaries
            if (beneficiaries != null && beneficiaries.Count >= 5) return new Tuple<bool, string>(false, "Cannot add more than 5 beneficiaries");

            //Add Beneficiary
            var beneficiary = _mapper.Map<Beneficiary>(request);
            beneficiary.IsActive = true;    
            await _unitOfWork.Repository<Beneficiary>().Add(beneficiary);
            bool isCompleted = await _unitOfWork.Complete();

            //In case of failure
            if (!isCompleted) return new Tuple<bool, string>(false, "Unable to add beneficiary");

            //Success
            return new Tuple<bool, string>(true, "Beneficiary added successfully");
        }

        /// <summary>
        /// Get Available Beneficiaries
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<BeneficiaryDetails>> GetBeneficiariesByUserIdAsync(int userId)
        {
            var response = await _beneficiaryRepository.GetBeneficiaryByUserIdAsync(userId);
            var mappedResponse = _mapper.Map<List<BeneficiaryDetails>>(response);
            return mappedResponse;
        }

        /// <summary>
        /// Get TopUp Options
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetTopUpOptionsAsync()
        {
            List<string> options = new List<string>();
            foreach(var ele in Constants.TopUpOptions)
            {
                options.Add("AED " + ele.ToString());
            }
            return options;
        }

        /// <summary>
        /// Top Up 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Tuple<bool, string>> TopUp(TopUpRequest request)
        {
            //Check if the user is valid
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if(user == null) return new Tuple<bool, string>(false, "User Not Found");

            //Fetch the user Balance from external Http Service
            var httpResponse = await _userTransactionHttpService.GetUserBalanceAsync(request.UserId);

            if (!httpResponse.IsSuccess) return new Tuple<bool, string>(false, "Unable to fetch data from HttpService");

            decimal userBalance = httpResponse.Data.Balance.HasValue? httpResponse.Data.Balance.Value : 0;

            // Check if user has sufficient balance
            if (userBalance < request.Amount + 1) // Adding 1 AED Transaction Fee
                return new Tuple<bool, string>(false, "Insufficient Balance");

            if (request.Amount <= 0 || !Constants.TopUpOptions.Contains(request.Amount)) return new Tuple<bool, string>(false, "TopUp amount not valid. Please select from available Top Up options");

            // Check monthly limit based on user verification status
            var monthlyLimit = user.IsVerified ? 500 : 1000;

            // Check if top-up exceeds monthly limit
            decimal? beneficiaryMonthlyTopUp = await CalculateMonthlyTotalBeneficiaryTopUpAmount(request.BeneficiaryId, request.Amount);

            if (beneficiaryMonthlyTopUp == null) return new Tuple<bool, string>(false, "Data not found");

            if (beneficiaryMonthlyTopUp != -1 &&  (beneficiaryMonthlyTopUp >monthlyLimit))
                return new Tuple<bool, string>(false, "Exceeds the monthly limit");

            // Check if the total top-up amount for all beneficiaries exceeds AED 3,000 per month
            var totalTopUpAmount = await CalculateMonthlyTotalTopUpAmount(request.UserId, request.Amount);

            if(totalTopUpAmount == -1) return new Tuple<bool, string>(false, "Data not found");

            if (totalTopUpAmount > 3000) return new Tuple<bool, string>(false, "Total top-up amount for all beneficiaries exceeds AED 3,000 per month");

            //Debit Transaction
            var debitTransactionRequest = new UserTransactionRequest()
            {
                Amount = request.Amount + 1,
                UserId = request.UserId,
            };
            var debitTransactionResponse = await _userTransactionHttpService.DebitCreditTransaction(debitTransactionRequest);
            if (!debitTransactionResponse.IsSuccess) return new Tuple<bool, string>(false, "Unable to fetch data from HttpService");

            //Perform Top Up
            var topUpRequest = _mapper.Map<TopUpTransaction>(request);

            await _transactionRepository.TopUp(topUpRequest);
            bool isCompleted = await _unitOfWork.Complete();
            if (!isCompleted) return new Tuple<bool, string>(false, "Unable to perform top Up");
            return new Tuple<bool, string>(true, "Top Up performed successfully");
        }

        #endregion


        #region "Private Methods"

        /// <summary>
        /// Calculate Monthly Top Up Amount For All Beneficiaries
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newTransactionAmount"></param>
        /// <returns></returns>
        private async Task<decimal?> CalculateMonthlyTotalTopUpAmount(int userId, decimal newTransactionAmount)
        {
            // Calculate the total top-up amount for the current month
            var currentDate = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Fetch all transactions for the current month
            var transactions = await _transactionRepository.GetTransactionsByUserId(userId);

            if(transactions == null) return -1;
            var totalTopUpAmount = transactions
                .Where(t => t.Amount > 0 && t.CreatedOn.Date >= firstDayOfMonth.Date && t.CreatedOn.Date <= lastDayOfMonth.Date)
                .Sum(t => t.Amount);

            // Add the new transaction amount
            totalTopUpAmount += newTransactionAmount;

            return totalTopUpAmount;
        }

        /// <summary>
        /// Calculate Monthly TopUp Transaction Current Month By Beneficiary Id
        /// </summary>
        /// <param name="beneficiaryId"></param>
        /// <param name="newTransactionAmount"></param>
        /// <returns></returns>
        private async Task<decimal?> CalculateMonthlyTotalBeneficiaryTopUpAmount(int beneficiaryId, decimal newTransactionAmount)
        {
            // Calculate the total top-up amount for the current month
            var currentDate = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Fetch all transactions for the current month
            var transactions = await _transactionRepository.GetTransactionsByBeneficiaryId(beneficiaryId);
            if (transactions == null) return -1;

            var totalTopUpAmount = transactions.Where(t => t.Amount > 0 && t.CreatedOn.Date >= firstDayOfMonth.Date && t.CreatedOn.Date <= lastDayOfMonth.Date)
                .Sum(t => t.Amount);

            // Add the new transaction amount
            totalTopUpAmount += newTransactionAmount;

            return totalTopUpAmount;
        }

        #endregion 
    }
}
