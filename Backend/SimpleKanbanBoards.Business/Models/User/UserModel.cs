using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Role;

namespace SimpleKanbanBoards.Business.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public IReadOnlyCollection<RoleModel>? Roles { get; set; }
    }
}
