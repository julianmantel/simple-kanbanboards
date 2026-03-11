using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.DataAccess.Repository.IRepository
{
    public interface ITaskRepository : IRepositoryBase<Models.Task>
    {
        Task AddUserTask(int userID, int taskID);
        Task<bool> IsTaskAssigned(int userID, int taskID);

        Task<int> CountTasksInColumnAsync(int columnId);
    }
}
