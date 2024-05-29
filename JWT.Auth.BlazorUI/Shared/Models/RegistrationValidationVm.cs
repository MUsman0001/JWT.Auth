using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace JWT.Auth.BlazorUI.Shared.Models
{
    public class RegistrationValidationVm : AbstractValidator<RegistrationVM>
    {
        public RegistrationValidationVm()
        {
            RuleFor(_ => _.FirstName).NotEmpty();
            RuleFor(_ => _.LastName).NotEmpty();
            RuleFor(_ => _.Email).EmailAddress().NotEmpty();
            RuleFor(_ => _.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");

            RuleFor(_ => _.ConfirmPassword).Equal(_ => _.Password).WithMessage("ConfirmPassword must equal Password");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<RegistrationVM>.CreateWithOptions((RegistrationVM)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
