using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.DataAccess.Repository
{
    public class RoleRepository(DbkanbanContext context): RepositoryBase<Role>(context), IRoleRepository
    {
    }
}
