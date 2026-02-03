using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.User;

namespace SimpleKanbanBoards.Business.Validators.User
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            int minPasswordLength = UserValidationRules.MinPasswordLength;

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password is required.")
                .MinimumLength(minPasswordLength).WithMessage($"New Password must be at least {minPasswordLength} characters long.")
                .Matches("[A-Z]").WithMessage("New Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("New Password must contain at least one digit.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
