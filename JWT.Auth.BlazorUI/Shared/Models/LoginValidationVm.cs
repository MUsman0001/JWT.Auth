﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace JWT.Auth.BlazorUI.Shared.Models
{
    
        public class LoginValidationVm : AbstractValidator<LoginVm>
        {
            public LoginValidationVm()
            {
                RuleFor(_ => _.Email).NotEmpty()
                   .EmailAddress()
                   .WithMessage("Invalid email");

                RuleFor(_ => _.Password).NotEmpty().WithMessage("Your password cannot be empty")
                        .MaximumLength(16).WithMessage("Invalid Password")
                        .Matches(@"[A-Z]+").WithMessage("Invalid Password")
                        .Matches(@"[a-z]+").WithMessage("Invalid Password")
                        .Matches(@"[0-9]+").WithMessage("Invalid Password")
                        .Matches(@"[\@\!\?\*\.]+").WithMessage("Invalid Password");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
            {
                var result = await ValidateAsync(ValidationContext<LoginVm>.CreateWithOptions((LoginVm)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }

