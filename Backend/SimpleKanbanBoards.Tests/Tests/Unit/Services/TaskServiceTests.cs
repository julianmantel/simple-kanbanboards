using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Business.Service;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Tests.Unit.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<IBoardColumnRepository> _boardColumnRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        _taskRepoMock = new Mock<ITaskRepository>();
        _boardColumnRepoMock = new Mock<IBoardColumnRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _sut = new TaskService(_taskRepoMock.Object, _boardColumnRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task CreateTask_WhenBoardColumnExistsAndWipLimitNotReached_ShouldAddTask()
    {
        var model = new CreateTaskModel
        {
            Title = "New Task",
            Description = "Task description",
            Priority = 1,
            ServiceClass = "Standard",
            IdBoardColumn = 1
        };

        var boardColumn = new BoardColumn { IdBoardColumn = 1, WipLimit = 5, IsEntry = false, IsDone = false };
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync(boardColumn);
        _taskRepoMock.Setup(r => r.CountTasksInColumnAsync(1))
                     .ReturnsAsync(3);

        await _sut.CreateTask(model);

        _taskRepoMock.Verify(r => r.AddAsync(It.Is<DataAccess.Models.Task>(t =>
            t.Title == model.Title &&
            t.Description == model.Description &&
            t.Priority == model.Priority &&
            t.ServiceClass == model.ServiceClass &&
            t.IdBoardColumn == model.IdBoardColumn
        )), Times.Once);
    }

    [Fact]
    public async Task CreateTask_WhenBoardColumnNotFound_ShouldThrowNotFoundException()
    {
        var model = new CreateTaskModel { Title = "Task", Description = "Desc", ServiceClass = "Expedite", Priority = 1, IdBoardColumn = 99 };
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync((BoardColumn?)null);

        var act = async () => await _sut.CreateTask(model);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Board column not found.");
    }

    [Fact]
    public async Task CreateTask_WhenWipLimitReachedAndNotExpedite_ShouldThrowConflictException()
    {
        var model = new CreateTaskModel
        {
            Title = "Task",
            Description = "Desc",
            ServiceClass = "Standard",
            Priority = 1,
            IdBoardColumn = 1
        };

        var boardColumn = new BoardColumn { IdBoardColumn = 1, WipLimit = 5, IsEntry = false, IsDone = false };
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync(boardColumn);
        _taskRepoMock.Setup(r => r.CountTasksInColumnAsync(1))
                     .ReturnsAsync(5);

        var act = async () => await _sut.CreateTask(model);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("Cannot move task. WIP limit for the column has been reached.");
    }

    [Fact]
    public async Task CreateTask_WhenWipLimitReachedButIsExpedite_ShouldAddTask()
    {
        var model = new CreateTaskModel
        {
            Title = "Task",
            Description = "Desc",
            ServiceClass = "Expedite",
            Priority = 1,
            IdBoardColumn = 1
        };

        var boardColumn = new BoardColumn { IdBoardColumn = 1, WipLimit = 5, IsEntry = false, IsDone = false };
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync(boardColumn);
        _taskRepoMock.Setup(r => r.CountTasksInColumnAsync(1))
                     .ReturnsAsync(5);

        await _sut.CreateTask(model);

        _taskRepoMock.Verify(r => r.AddAsync(It.IsAny<DataAccess.Models.Task>()), Times.Once);
    }

    [Fact]
    public async Task CreateTask_WhenColumnIsEntry_ShouldIgnoreWipLimit()
    {
        var model = new CreateTaskModel
        {
            Title = "Task",
            Description = "Desc",
            ServiceClass = "Standard",
            Priority = 1,
            IdBoardColumn = 1
        };

        var boardColumn = new BoardColumn { IdBoardColumn = 1, WipLimit = 5, IsEntry = true, IsDone = false };
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync(boardColumn);
        _taskRepoMock.Setup(r => r.CountTasksInColumnAsync(1))
                     .ReturnsAsync(10);

        await _sut.CreateTask(model);

        _taskRepoMock.Verify(r => r.AddAsync(It.IsAny<DataAccess.Models.Task>()), Times.Once);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskExists_ShouldReturnMappedModel()
    {
        var task = new DataAccess.Models.Task
        {
            IdTask = 1,
            Title = "Test Task",
            Description = "Description",
            Priority = 3,
            ServiceClass = "Expedite",
            IdBoardColumn = 5,
            IdUser = 10,
            CreatedAt = new DateTime(2024, 1, 1),
            CompletedAt = null
        };

        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(task);

        var result = await _sut.GetTaskById(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(task.IdTask);
        result.Title.Should().Be(task.Title);
        result.Description.Should().Be(task.Description);
        result.Priority.Should().Be(task.Priority);
        result.ServiceClass.Should().Be(task.ServiceClass);
        result.IdBoardColumn.Should().Be(task.IdBoardColumn);
        result.IdUser.Should().Be(task.IdUser);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync((DataAccess.Models.Task?)null);

        var act = async () => await _sut.GetTaskById(999);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Task not found.");
    }

    [Fact]
    public async Task GetTasksByColumnIdAsync_WhenTasksExist_ShouldReturnMappedModels()
    {
        var tasks = new List<DataAccess.Models.Task>
        {
            new() { IdTask = 1, Title = "Task 1", Description = "Desc 1", Priority = 1, ServiceClass = "Standard", IdBoardColumn = 1 },
            new() { IdTask = 2, Title = "Task 2", Description = "Desc 2", Priority = 2, ServiceClass = "Expedite", IdBoardColumn = 1 }
        };

        _taskRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>(), It.IsAny<Func<IQueryable<DataAccess.Models.Task>, IOrderedQueryable<DataAccess.Models.Task>>>(), It.IsAny<Expression<Func<DataAccess.Models.Task, object>>[]>()))
                     .ReturnsAsync(tasks);

        var result = await _sut.GetTasksByColumnIdAsync(1);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetTasksByColumnIdAsync_WhenNoTasks_ShouldThrowNotFoundException()
    {
        _taskRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>(), It.IsAny<Func<IQueryable<DataAccess.Models.Task>, IOrderedQueryable<DataAccess.Models.Task>>>(), It.IsAny<Expression<Func<DataAccess.Models.Task, object>>[]>()))
                     .ReturnsAsync(new List<DataAccess.Models.Task>());

        var act = async () => await _sut.GetTasksByColumnIdAsync(1);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("No tasks found for the specified column.");
    }

    [Fact]
    public async Task UpdateTask_WhenTaskExists_ShouldUpdateFields()
    {
        var existingTask = new DataAccess.Models.Task
        {
            IdTask = 1,
            Title = "Old Title",
            Description = "Old Desc",
            Priority = 1,
            ServiceClass = "Standard",
            IdBoardColumn = 1
        };

        var updateModel = new UpdateTaskModel
        {
            Id = 1,
            Title = "New Title",
            Description = "New Desc",
            Priority = 5,
            ServiceClass = "Expedite",
            IdBoardColumn = 2,
            CompletedAt = DateTime.Now
        };

        var boardColumn = new BoardColumn { IdBoardColumn = 2, WipLimit = 5, IsEntry = false, IsDone = false };

        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(existingTask);
        _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                            .ReturnsAsync(boardColumn);
        _taskRepoMock.Setup(r => r.CountTasksInColumnAsync(2))
                     .ReturnsAsync(2);

        await _sut.UpdateTask(updateModel);

        existingTask.Title.Should().Be("New Title");
        existingTask.Description.Should().Be("New Desc");
        existingTask.Priority.Should().Be(5);
        existingTask.ServiceClass.Should().Be("Expedite");
        existingTask.IdBoardColumn.Should().Be(2);
        existingTask.CompletedAt.Should().Be(updateModel.CompletedAt);
        _taskRepoMock.Verify(r => r.Update(existingTask), Times.Once);
    }

    [Fact]
    public async Task UpdateTask_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        var updateModel = new UpdateTaskModel { Id = 99, Title = "Title", Description = "Desc", ServiceClass = "Expedite", Priority = 1, IdBoardColumn = 1 };
        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync((DataAccess.Models.Task?)null);

        var act = async () => await _sut.UpdateTask(updateModel);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Task not found");
        _taskRepoMock.Verify(r => r.Update(It.IsAny<DataAccess.Models.Task>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTask_WhenTaskExists_ShouldCallRemove()
    {
        var task = new DataAccess.Models.Task { IdTask = 1, Title = "Task" };
        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(task);

        await _sut.DeleteTask(1);

        _taskRepoMock.Verify(r => r.Remove(task), Times.Once);
    }

    [Fact]
    public async Task DeleteTask_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        _taskRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync((DataAccess.Models.Task?)null);

        var act = async () => await _sut.DeleteTask(999);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Task not found");
    }

    [Fact]
    public async Task AddUserTask_WhenTaskAndUserExist_ShouldAddUserToTask()
    {
        var userTask = new UserTaskModel { IdTask = 1, IdUser = 2 };

        _taskRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(true);
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _taskRepoMock.Setup(r => r.IsTaskAssigned(2, 1))
                     .ReturnsAsync(false);

        await _sut.AddUserTask(userTask);

        _taskRepoMock.Verify(r => r.AddUserTask(2, 1), Times.Once);
    }

    [Fact]
    public async Task AddUserTask_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        var userTask = new UserTaskModel { IdTask = 99, IdUser = 2 };

        _taskRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(false);

        var act = async () => await _sut.AddUserTask(userTask);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Task not found");
    }

    [Fact]
    public async Task AddUserTask_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        var userTask = new UserTaskModel { IdTask = 1, IdUser = 99 };

        _taskRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(true);
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(false);

        var act = async () => await _sut.AddUserTask(userTask);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("User not found");
    }

    [Fact]
    public async Task AddUserTask_WhenTaskAlreadyAssigned_ShouldThrowConflictException()
    {
        var userTask = new UserTaskModel { IdTask = 1, IdUser = 2 };

        _taskRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<DataAccess.Models.Task, bool>>>()))
                     .ReturnsAsync(true);
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _taskRepoMock.Setup(r => r.IsTaskAssigned(2, 1))
                     .ReturnsAsync(true);

        var act = async () => await _sut.AddUserTask(userTask);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("Task is already assigned to the user");
    }
}
