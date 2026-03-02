using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Service;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Tests.Unit.Services;

public class BoardServiceTests
{
    private readonly Mock<IBoardRepository> _boardRepoMock;
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly BoardService _sut;

    public BoardServiceTests()
    {
        _boardRepoMock = new Mock<IBoardRepository>();
        _projectRepoMock = new Mock<IProjectRepository>();
        _sut = new BoardService(_boardRepoMock.Object, _projectRepoMock.Object);
    }

    [Fact]
    public async Task CreateBoardAsync_WhenProjectExists_ShouldAddBoard()
    {
        var model = new BoardModel { Name = "Sprint 1", Description = "First sprint", ProjectId = 10 };
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(true);

        await _sut.CreateBoardAsync(model);

        _boardRepoMock.Verify(r => r.AddAsync(It.Is<Board>(b =>
            b.BoardName == model.Name &&
            b.Description == model.Description &&
            b.IdProject == model.ProjectId &&
            b.IsActive == true
        )), Times.Once);
    }

    [Fact]
    public async Task CreateBoardAsync_WhenProjectDoesNotExist_ShouldThrowNotFoundException()
    {
        var model = new BoardModel { Name = "Sprint 1", ProjectId = 99 };
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(false);

        var act = async () => await _sut.CreateBoardAsync(model);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Project does not exist.");
        _boardRepoMock.Verify(r => r.AddAsync(It.IsAny<Board>()), Times.Never);
    }

    [Fact]
    public async Task GetBoardByIdAsync_WhenBoardExists_ShouldReturnMappedModel()
    {
        var board = new Board
        {
            IdBoard = 1,
            BoardName = "Backlog",
            Description = "All tasks",
            CreatedAt = new DateOnly(2024, 1, 15),
            IsActive = true,
            IdProject = 5
        };
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync(board);

        var result = await _sut.GetBoardByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(board.IdBoard);
        result.Name.Should().Be(board.BoardName);
        result.Description.Should().Be(board.Description);
        result.Is_Active.Should().BeTrue();
        result.ProjectId.Should().Be(board.IdProject);
    }

    [Fact]
    public async Task GetBoardByIdAsync_WhenBoardDoesNotExist_ShouldThrowNotFoundException()
    {
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync((Board?)null);

        var act = async () => await _sut.GetBoardByIdAsync(999);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Board not found.");
    }

    [Fact]
    public async Task GetBoardByIdAsync_WhenBoardHasNullOptionals_ShouldReturnDefaults()
    {
        var board = new Board { IdBoard = 2, BoardName = "Board", CreatedAt = null, IsActive = null, IdProject = null };
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync(board);

        var result = await _sut.GetBoardByIdAsync(2);

        result.Is_Active.Should().BeFalse();
        result.ProjectId.Should().Be(0);
        result.Created_At.Should().Be(DateOnly.FromDateTime(DateTime.Now), because: "should default to today");
    }

    [Fact]
    public async Task ToggleBoardAsync_WhenBoardExists_ShouldCallToggle()
    {
        var board = new Board { IdBoard = 1, IsActive = true };
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync(board);

        await _sut.ToggleBoardAsync(1);

        _boardRepoMock.Verify(r => r.ToggleBoardAsync(board), Times.Once);
    }

    [Fact]
    public async Task ToggleBoardAsync_WhenBoardDoesNotExist_ShouldThrowNotFoundException()
    {
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync((Board?)null);

        var act = async () => await _sut.ToggleBoardAsync(42);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Board not found.");
        _boardRepoMock.Verify(r => r.ToggleBoardAsync(It.IsAny<Board>()), Times.Never);
    }

    [Fact]
    public async Task UpdateBoardAsync_WhenBoardExists_ShouldUpdateFieldsAndCallUpdate()
    {
        var board = new Board { IdBoard = 1, BoardName = "Old Name", Description = "Old desc" };
        var updateModel = new UpdateBoardModel { Id = 1, Name = "New Name", Description = "New desc" };

        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync(board);

        await _sut.UpdateBoardAsync(updateModel);

        board.BoardName.Should().Be("New Name");
        board.Description.Should().Be("New desc");
        _boardRepoMock.Verify(r => r.Update(board), Times.Once);
    }

    [Fact]
    public async Task UpdateBoardAsync_WhenBoardDoesNotExist_ShouldThrowNotFoundException()
    {
        _boardRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Board, bool>>>()))
                      .ReturnsAsync((Board?)null);

        var updateModel = new UpdateBoardModel { Id = 99, Name = "X" };

        var act = async () => await _sut.UpdateBoardAsync(updateModel);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Board not found.");
        _boardRepoMock.Verify(r => r.Update(It.IsAny<Board>()), Times.Never);
    }
}
