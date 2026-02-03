using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Validators.Board
{
    public static class BoardValidationRules
    {
        public const int BoardNameMinLength = 3;
        public const int BoardNameMaxLength = 160;
        public const int BoardDescriptionMaxLength = 500;
    }
}
