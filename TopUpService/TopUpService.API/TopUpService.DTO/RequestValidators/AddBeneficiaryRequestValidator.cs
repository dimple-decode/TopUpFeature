using FluentValidation;
using TopUpService.DTO.Request;

namespace TopUpService.DTO.RequestValidators
{
    /// <summary>
    /// Add Beneficiary Request Object Validator
    /// </summary>
    public class AddBeneficiaryRequestValidator:AbstractValidator<AddBeneficiaryRequest>
    {
        public AddBeneficiaryRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
            RuleFor(x => x.NickName).NotNull().WithMessage("NickName is required").MaximumLength(20).WithMessage("NickName cannot be more than 20 characters");
        }
    }
}
