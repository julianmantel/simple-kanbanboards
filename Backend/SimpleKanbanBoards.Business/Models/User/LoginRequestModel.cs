using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Role;

namespace SimpleKanbanBoards.Business.Models.User
{
    public class LoginRequestModel
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
