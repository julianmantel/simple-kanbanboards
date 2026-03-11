using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Validators.Board
{
    public static class BoardValidationRules
    {
        public const int BOARDNAME_MIN_LENGTH = 3;
        public const int BOARDNAME_MAX_LENGTH = 160;
        public const int BOARDDESCRIPTION_MAX_LENGTH = 500;
    }
}
