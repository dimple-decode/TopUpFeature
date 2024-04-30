using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TransactionService.DTO.Request;
using TransactionService.DTO.Response;
using TransactionService.Logic;

namespace TransactionService.API.Controllers
{
    /// <summary>
    /// Controller for User Transactions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserTransactionController : ControllerBase
    {
        private readonly IUserTransactionService _userTransactionService;
        private readonly IValidator<UserTransactionRequest> _userTransactionRequestValidator;

        public UserTransactionController(IUserTransactionService userTransactionService, IValidator<UserTransactionRequest> userTransactionRequestValidator)
        {
            _userTransactionService = userTransactionService;
            _userTransactionRequestValidator = userTransactionRequestValidator;
        }

        /// <summary>
        /// Get User Balance By UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getUserBalance/{userId}")]
        public async Task<ActionResult<UserTransactionResponse>> GetUserBalanceByUserId([FromRoute] int userId)
        {
            var result = await _userTransactionService.GetUserBalanceById(userId);
            if (!result.HasValue) return BadRequest(new UserTransactionResponse() { ErrorMessage = "User not found"});

            var response = new UserTransactionResponse()
            {
                Data = new UserBalance() { Balance = result.Value },
                IsSuccess = true
            };

            return Ok(response);
        }

        /// <summary>
        /// Debit or Credit Transaction Based on UserId and Transaction Type (0 - Debit, 1- Credit)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("debitCreditTransaction")]
        public async Task<ActionResult<UserTransactionResponse>> DebitCreditTransaction([FromBody] UserTransactionRequest request)
        {
            var validationResult = _userTransactionRequestValidator.Validate(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(x =>
                                                                {
                                                                    return new HttpResponseModel()
                                                                    {
                                                                        ErrorCode = x.ErrorCode,
                                                                        ErrorMessage = x.ErrorMessage
                                                                    };
                                                                }
                                                                ));

            var response = new HttpResponseModel();

            var result = await _userTransactionService.DebitCreditTransaction(request);
            if (!result.Item1)
            {
                response.ErrorMessage = result.Item2;
                return BadRequest(response);
            }

            response.IsSuccess = true;
            response.Message = result.Item2;

            return Ok(response);
        }

    }
}
