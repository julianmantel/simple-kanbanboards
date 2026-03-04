using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.Business.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        private readonly IUserRepository _userRepository;
        private int _minUserNameLength = UserValidationRules.MinUserNameLength;
        private int _maxUserNameLength = UserValidationRules.MaxUserNameLength;
        private int _minPasswordLength = UserValidationRules.MinPasswordLength;

        public CreateUserValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty()
                    .WithMessage("UserName is required.")
                .MinimumLength(_minUserNameLength)
                    .WithMessage($"UserName must be at least {_minUserNameLength} characters long.")
                .MaximumLength(_maxUserNameLength)
                    .WithMessage($"UserName must not exceed {_maxUserNameLength} characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(_minPasswordLength)
                    .WithMessage($"Password must be at least {_minPasswordLength} characters long.")
                .Matches("[A-Z]")
                    .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                    .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                    .WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email is required.")
                .EmailAddress()
                    .WithMessage("A valid email is required.");
        }
    }
}
