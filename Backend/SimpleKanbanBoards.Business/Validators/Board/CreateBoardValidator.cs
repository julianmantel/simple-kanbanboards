using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SimpleKanbanBoards.Business.Models.Board;

namespace SimpleKanbanBoards.Business.Validators.Board
{
    public class CreateBoardValidator : AbstractValidator<CreateBoardModel>
    {
        private int minNameLength = BoardValidationRules.BoardNameMinLength;
        private int maxNameLength = BoardValidationRules.BoardNameMaxLength;
        private int maxDescriptionLength = BoardValidationRules.BoardDescriptionMaxLength;

        public CreateBoardValidator()
        {
            RuleFor(board => board.Name)
                .NotEmpty().WithMessage("Board name is required.")
                .MinimumLength(minNameLength).WithMessage($"Board name must be at least {minNameLength} characters long.")
                .MaximumLength(maxNameLength).WithMessage($"Board name must not exceed {maxNameLength} characters.");

            RuleFor(board => board.Description)
                .MaximumLength(maxDescriptionLength).WithMessage($"Board description must not exceed {maxDescriptionLength} characters.");
        }
    }
}
