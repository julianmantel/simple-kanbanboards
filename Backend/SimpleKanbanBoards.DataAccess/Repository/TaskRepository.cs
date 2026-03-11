using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.DataAccess.Repository
{
    public class TaskRepository(DbkanbanContext context) : RepositoryBase<Models.Task>(context), ITaskRepository
    {
        public async Task AddUserTask(int userID, int taskID)
        {
            var task = await _context.Tasks.FindAsync(taskID);

            task.IdUser = userID;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTaskAssigned(int userID, int taskID) => await _context.Tasks.AnyAsync(t => t.IdTask == taskID && t.IdUser == userID);

        public async Task<int> CountTasksInColumnAsync(int columnId) => await _context.Tasks.CountAsync(t => t.IdBoardColumn == columnId);
    }
}
