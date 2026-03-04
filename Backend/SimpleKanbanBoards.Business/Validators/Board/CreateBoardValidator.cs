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
        private int _minNameLength = BoardValidationRules.BoardNameMinLength;
        private int _maxNameLength = BoardValidationRules.BoardNameMaxLength;
        private int _maxDescriptionLength = BoardValidationRules.BoardDescriptionMaxLength;

        public CreateBoardValidator()
        {
            RuleFor(board => board.Name)
                .NotEmpty()
                    .WithMessage("Board name is required.")
                .MinimumLength(_minNameLength)
                    .WithMessage($"Board name must be at least {_minNameLength} characters long.")
                .MaximumLength(_maxNameLength)
                    .WithMessage($"Board name must not exceed {_maxNameLength} characters.");

            RuleFor(board => board.Description)
                .MaximumLength(_maxDescriptionLength)
                    .WithMessage($"Board description must not exceed {_maxDescriptionLength} characters.");
        }
    }
}
