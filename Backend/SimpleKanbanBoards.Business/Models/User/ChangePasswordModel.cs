using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.User
{
    public class ChangePasswordModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
