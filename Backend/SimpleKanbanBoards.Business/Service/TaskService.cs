using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Business.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IBoardColumnRepository boardColumnRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _boardColumnRepository = boardColumnRepository;
            _userRepository = userRepository;
        }

        public async Task AddUserTask(UserTaskModel userTask)
        {
            bool existTask = await _taskRepository.Exist(t => t.IdTask == userTask.IdTask);
            if (!existTask)
            {
                throw new NotFoundException("Task not found");
            }

            bool existUser = await _userRepository.Exist(u => u.IdUser == userTask.IdUser);
            if (!existUser)
            {
                throw new NotFoundException("User not found");
            }

            bool isTaskAlreadyAssigned = await _taskRepository.IsTaskAssigned(userTask.IdUser, userTask.IdTask);
            if (isTaskAlreadyAssigned)
            {
                throw new ConflictException("Task is already assigned to the user");
            }

            await _taskRepository.AddUserTask(userTask.IdUser, userTask.IdTask);
        }

        public async Task CreateTask(CreateTaskModel task)
        {
            await CheckConflicts(task.IdBoardColumn, task.ServiceClass);

            var newTask = new DataAccess.Models.Task
            {
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                Priority = task.Priority,
                ServiceClass = task.ServiceClass,
                IdBoardColumn = task.IdBoardColumn
            };

            await _taskRepository.AddAsync(newTask);
        }

        public async Task DeleteTask(int id)
        {
            var task = await _taskRepository.GetFirstOrDefault(t => t.IdTask == id);
            if (task == null)
            {
                throw new NotFoundException("Task not found");
            }

            _taskRepository.Remove(task);
        }

        public async Task<TaskModel> GetTaskById(int id)
        {
            var task = await _taskRepository.GetFirstOrDefault(t => t.IdTask == id);

            if (task == null)
            {
                throw new NotFoundException("Task not found.");
            }

            return new TaskModel
            {
                Id = task.IdTask,
                IdUser = task.IdUser ?? 0,
                IdBoardColumn = task.IdBoardColumn ?? 0,
                Title = task.Title ?? "Tittle",
                Description = task.Description ?? string.Empty,
                CreatedAt = task.CreatedAt ?? DateTime.MinValue,
                CompletedAt = task.CompletedAt,
                Priority = task.Priority ?? 0,
                ServiceClass = task.ServiceClass
            };
        }

        public async Task UpdateTask(UpdateTaskModel task)
        {
            var oldTask = await _taskRepository.GetFirstOrDefault(t => t.IdTask == task.Id);
            if (oldTask == null)
            {
                throw new NotFoundException("Task not found");
            }

            await CheckConflicts(task.IdBoardColumn, task.ServiceClass);

            oldTask.Title = task.Title;
            oldTask.Description = task.Description;
            oldTask.CompletedAt = task.CompletedAt;
            oldTask.Priority = task.Priority;
            oldTask.ServiceClass = task.ServiceClass;
            oldTask.IdBoardColumn = task.IdBoardColumn;

            _taskRepository.Update(oldTask);
        }

        private async Task CheckConflicts(int boardColumnId, string serviceClass)
        {
            var boardColumn = await _boardColumnRepository.GetFirstOrDefault(bc => bc.IdBoardColumn == boardColumnId);
            if (boardColumn == null)
            {
                throw new NotFoundException("Board column not found.");
            }

            bool hasReachedLimit = await _taskRepository.CountTasksInColumnAsync(boardColumnId) >= boardColumn.WipLimit;
            bool isEntryOrDoneColumn = boardColumn.IsEntry == true || boardColumn.IsDone == true;
            bool isExpedite = (serviceClass == "Expedite");
            if (hasReachedLimit && !isEntryOrDoneColumn)
            {
                throw new ConflictException("Cannot move task. WIP limit for the column has been reached.");
            }

        }
    }
}
