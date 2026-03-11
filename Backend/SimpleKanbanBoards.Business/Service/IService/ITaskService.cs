using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Task;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface ITaskService
    {
        Task CreateTask(CreateTaskModel task);
        Task UpdateTask(UpdateTaskModel task);
        Task DeleteTask(int id);
        Task<TaskModel> GetTaskById(int id);
        Task AddUserTask(UserTaskModel userTask);
    }
}
