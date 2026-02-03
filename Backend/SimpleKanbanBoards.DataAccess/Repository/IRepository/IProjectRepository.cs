using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.DataAccess.Repository.IRepository
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        Task AddUserProject(UserProject userProject);
        int CountUsers(int projectID);
        Task<bool> IsUserInProject(int userId);
    }
}
