using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Validators.User
{
    public static class UserValidationRules
    {
        public const int MinUserNameLength = 3;
        public const int MaxUserNameLength = 25;
        public const int MinPasswordLength = 4;
    }
}
