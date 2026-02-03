using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.User;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.Business.Validators.User
{
    public class ResetPassRequestValidator : AbstractValidator<ResetPassRequestModel>
    {
        public ResetPassRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }
}
