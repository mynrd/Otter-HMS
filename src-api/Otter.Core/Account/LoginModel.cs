using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Core.Account
{
    public class LoginModel : IValidate
    {
        public string CompanyCode { get; set; }
        public string SiteCode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.CompanyCode).MinimumLength(3).NotNull().NotEmpty();
            RuleFor(x => x.SiteCode).MinimumLength(3).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).MinimumLength(3).NotEmpty().NotNull();
        }
    }
}
