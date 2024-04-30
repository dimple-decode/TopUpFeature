using FluentValidation;
using TransactionService.Common;
using TransactionService.DTO.Request;

namespace TransactionService.DTO.RequestValidators
{
    /// <summary>
    /// User Transaction Request Validator
    /// </summary>
    public class UserTransactionRequestValidator : AbstractValidator<UserTransactionRequest>
    {
        public UserTransactionRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("User Id should not be null");
            RuleFor(x => x.Amount).NotNull().GreaterThan(0).WithMessage("Entered amount should be greater than 0");
            RuleFor(x => x.TransactionType).Must(i => Enum.IsDefined(typeof(TransactionType), i)).WithMessage("Incorrect Value. Value should be specified either 0 for Debit or 1 for Credit");
        }
    }
}
