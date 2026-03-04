using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.BoardColumn;

namespace SimpleKanbanBoards.Business.Validators.BoardColumn
{
    public class CreateBoardColumnValidator : AbstractValidator<CreateBoardColumnModel>
    {
        private int _maxNameLength = BoardColumnValidationRules.MaxNameLength;

        public CreateBoardColumnValidator()
        {
            RuleFor(bc => bc.Name)
                .NotEmpty()
                    .WithMessage("Column name is required.")
                .MaximumLength(_maxNameLength)
                    .WithMessage($"Column name cannot exceed {_maxNameLength} characters.");
            RuleFor(bc => bc.Position)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Position must be a non-negative integer.");
            RuleFor(bc => bc.WipLimit)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("WIP Limit must be a non-negative integer.");
        }
    }
}
