using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using TopUpService.Common;
using TopUpService.DTO;
using TopUpService.DTO.Response;
using TopUpService.Logic;

namespace TopUpService.Logic
{
    /// <summary>
    /// User Transaction Http Service
    /// </summary>
    public class UserTransactionHttpService:IUserTransactionHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UserTransactionHttpService(HttpClient httpClient, IConfiguration config)
        {
            _config = config;

            _httpClient = httpClient;
            string baseAddress = _config["TransactionServiceSettings:BaseAddress"];   
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(Constants.ContentTypeHeader, "application/json");


        }

        /// <summary>
        /// Get User Balance
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserTransactionResponse> GetUserBalanceAsync(int userId)
        {
            var result = await _httpClient.GetFromJsonAsync<UserTransactionResponse>($"getUserBalance/{userId}");
            return result;
        }

        /// <summary>
        /// Service to debit or credit the amount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserTransactionResponse> DebitCreditTransaction(UserTransactionRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("debitCreditTransaction",request);
            var  result = await response.Content.ReadFromJsonAsync<UserTransactionResponse>();
            return result;
        }
    }
}
