using SimpleKanbanBoards.Business.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IRoleService
    {
        Task<IReadOnlyCollection<RoleModel>> GetRolesAsync();
    }
}
