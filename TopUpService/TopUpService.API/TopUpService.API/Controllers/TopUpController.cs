using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopUpService.DTO.Request;
using TopUpService.DTO.Response;
using TopUpService.Logic;

namespace TopUpService.API.Controllers
{
    /// <summary>
    /// Top Up Controller
    /// </summary>
    [Route("api/[controller]")]
    public class TopUpController : ControllerBase
    {
        private readonly IUserTopUpService _topUpService;
        private readonly IValidator<AddBeneficiaryRequest> _addBeneficiaryRequestValidator;


        public TopUpController(IUserTopUpService topUpService, IValidator<AddBeneficiaryRequest> addBeneficiaryRequestValidator)
        {
            _topUpService = topUpService;
            _addBeneficiaryRequestValidator = addBeneficiaryRequestValidator;
        }

        /// <summary>
        /// Add TopUp Beneficiary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("addBeneficiary")]
        public async Task<ActionResult<AddBeneficiaryResponse>> AddBeneficiary([FromBody] AddBeneficiaryRequest request)
        {
            var validationResult = _addBeneficiaryRequestValidator.Validate(request);

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

            var response = new AddBeneficiaryResponse();

            var result = await _topUpService.AddBeneficiaryAsync(request);
            if (!result.Item1)
            {
                response.ErrorMessage = result.Item2;
                return BadRequest(response);
            }

            response.IsSuccess = true;
            response.Message = result.Item2;
           
            return Ok(response);
        }

        /// <summary>
        /// Get Beneficiaries
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getBeneficiaries/{userId}")]
        public async Task<ActionResult<GetBeneficiaryResponse>> GetBeneficiaries([FromRoute]int userId)
        {
            var beneficiaries = await _topUpService.GetBeneficiariesByUserIdAsync(userId);
            var response = new GetBeneficiaryResponse()
            {
                Data = beneficiaries,
                IsSuccess = true
            };
            return Ok(response);
        }

        /// <summary>
        /// Get Top Up Options
        /// </summary>
        /// <returns></returns>
        [HttpGet("getTopUpOptions")]
        public async Task<ActionResult<TopUpOptionsResponse>> GetTopUpOptions()
        {
            var topUpOptions = await _topUpService.GetTopUpOptionsAsync();
            var response = new TopUpOptionsResponse()
            {
                Data = topUpOptions,
                IsSuccess = true
            };
            return response;
        }

        [HttpPost("topUp")]
        public async Task<ActionResult<TopUpResponse>> TopUp([FromBody]TopUpRequest request)
        {
            var response = new TopUpResponse();
            var result = await _topUpService.TopUp(request);
            if (!result.Item1) {
                response.ErrorMessage = result.Item2;
                return BadRequest(response); 
            }
           response.IsSuccess = true;
           response.Message = result.Item2;

            return Ok(response);
        }
     
    }
}
