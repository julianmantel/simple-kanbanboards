using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;

namespace SimpleKanbanBoards.DataAccess.Repository
{
    public class UserRepository(DbkanbanContext context) : RepositoryBase<User>(context), IUserRepository;
}
