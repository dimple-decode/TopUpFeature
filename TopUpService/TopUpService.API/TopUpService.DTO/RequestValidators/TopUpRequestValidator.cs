using FluentValidation;
using TopUpService.DTO.Request;

namespace TopUpService.DTO.RequestValidators
{
    /// <summary>
    /// Top Up Request Validator
    /// </summary>
    public class TopUpRequestValidator: AbstractValidator<TopUpRequest>
    {
        public TopUpRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("User Id is required");
            RuleFor(x => x.Amount).NotNull().WithMessage("Amount is required");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Entered Amount should be greater than 0");
        }
    }
}
