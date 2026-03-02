using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.Project;
using SimpleKanbanBoards.Business.Service;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Tests.Unit.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly ProjectService _sut;

    public ProjectServiceTests()
    {
        _projectRepoMock = new Mock<IProjectRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _sut = new ProjectService(_userRepoMock.Object, _projectRepoMock.Object);
    }

    [Fact]
    public async Task GetProjectByIDAsync_WhenProjectExists_ShouldReturnMappedModel()
    {
        var project = new Project
        {
            IdProject = 1,
            Title = "Proyecto anti therians",
            Description = "nose",
            StartDate = new DateOnly(2024, 1, 1),
            EndDate = new DateOnly(2024, 6, 30),
            MaxDevs = 5,
            Boards = new List<Board>
            {
                new Board { IdBoard = 10, BoardName = "Sprint 1", Description = "First sprint" }
            }
        };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>(),
                It.IsAny<Expression<Func<Project, object>>[]>()))
            .ReturnsAsync(project);

        var result = await _sut.GetProjectByIDAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Proyecto anti therians");
        result.Description.Should().Be("nose");
        result.MaxDevs.Should().Be(5);
        result.Boards.Should().HaveCount(1);
        result.Boards.First().Id.Should().Be(10);
    }

    [Fact]
    public async Task GetProjectByIDAsync_WhenProjectDoesNotExist_ShouldThrowNotFoundException()
    {
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>(),
                It.IsAny<Expression<Func<Project, object>>[]>()))
            .ReturnsAsync((Project?)null);

        var act = async () => await _sut.GetProjectByIDAsync(999);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Project does not exist.");
    }

    [Fact]
    public async Task CreateProjectAsync_WhenTitleIsUnique_ShouldCreateProjectAndAddManager()
    {
        var manager = new User { IdUser = 1, Username = "Manager" };

        var model = new CreateProjectModel
        {
            Title = "Half-Life 3",
            Description = "Valve saca ya la parte 3 de Half-Life y mi alma sera tuya",
            MaxDevs = 3,
            AuthorId = 1
        };

        Project? createdProject = null;

        _projectRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Project>()))
            .Callback<Project>(p => createdProject = p)
            .Returns(Task.CompletedTask);

        _projectRepoMock
            .Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(() => createdProject);

        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(false);

        _userRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(manager);

        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _projectRepoMock.Setup(r => r.IsUserInProject(It.IsAny<int>())).ReturnsAsync(false);
        _projectRepoMock.Setup(r => r.CountUsers(It.IsAny<int>())).Returns(0);

        await _sut.CreateProjectAsync(model);

        _projectRepoMock.Verify(r => r.AddAsync(It.Is<Project>(p =>
            p.Title == model.Title &&
            p.Description == model.Description &&
            p.MaxDevs == model.MaxDevs
        )), Times.Once);
        _projectRepoMock.Verify(r => r.AddUserProject(It.IsAny<UserProject>()), Times.Once);
    }

    [Fact]
    public async Task CreateProjectAsync_WhenTitleAlreadyExists_ShouldThrowConflictException()
    {
        var model = new CreateProjectModel { Title = "Naruto", Description = "asd", MaxDevs = 2, AuthorId = 1 };
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(true);

        var act = async () => await _sut.CreateProjectAsync(model);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("Project with the same title already exists.");
        _projectRepoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public async Task AddDevToProjectAsync_WhenAllConditionsPass_ShouldAddUserToProject()
    {
        var request = new ProjectUserModel { IdProject = 1, IdDev = 2 };
        var project = new Project { IdProject = 1, MaxDevs = 5 };

        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(project);
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _projectRepoMock.Setup(r => r.IsUserInProject(2)).ReturnsAsync(false);
        _projectRepoMock.Setup(r => r.CountUsers(1)).Returns(2);

        await _sut.AddDevToProjectAsync(request);

        _projectRepoMock.Verify(r => r.AddUserProject(It.Is<UserProject>(up =>
            up.IdProject == 1 && up.IdUser == 2
        )), Times.Once);
    }

    [Fact]
    public async Task AddDevToProjectAsync_WhenProjectNotFound_ShouldThrowNotFoundException()
    {
        var request = new ProjectUserModel { IdProject = 99, IdDev = 2 };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync((Project?)null);

        var act = async () => await _sut.AddDevToProjectAsync(request);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Project does not exist.");
    }

    [Fact]
    public async Task AddDevToProjectAsync_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        var request = new ProjectUserModel { IdProject = 1, IdDev = 99 };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(new Project { IdProject = 1, MaxDevs = 5 });
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(false);

        var act = async () => await _sut.AddDevToProjectAsync(request);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task AddDevToProjectAsync_WhenUserAlreadyInProject_ShouldThrowConflictException()
    {
        var request = new ProjectUserModel { IdProject = 1, IdDev = 2 };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(new Project { IdProject = 1, MaxDevs = 5 });
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _projectRepoMock.Setup(r => r.IsUserInProject(2)).ReturnsAsync(true);

        var act = async () => await _sut.AddDevToProjectAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("User is already assigned to this project.");
    }

    [Fact]
    public async Task AddDevToProjectAsync_WhenProjectIsAtMaxCapacity_ShouldThrowConflictException()
    {
        var request = new ProjectUserModel { IdProject = 1, IdDev = 2 };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(new Project { IdProject = 1, MaxDevs = 2 });
        _userRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<User, bool>>>()))
                     .ReturnsAsync(true);
        _projectRepoMock.Setup(r => r.IsUserInProject(2)).ReturnsAsync(false);
        _projectRepoMock.Setup(r => r.CountUsers(1)).Returns(3);

        var act = async () => await _sut.AddDevToProjectAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("Project has reached the maximum number of developers.");
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenProjectExists_ShouldCallRemove()
    {
        var project = new Project { IdProject = 1 };
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(project);

        await _sut.DeleteProjectAsync(1);

        _projectRepoMock.Verify(r => r.Remove(project), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenProjectNotFound_ShouldThrowNotFoundException()
    {
        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync((Project?)null);

        var act = async () => await _sut.DeleteProjectAsync(99);

        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage("Project does not exist.");
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenProjectExistsAndTitleUnique_ShouldUpdate()
    {
        var existing = new Project { IdProject = 1, Title = "Thunderbolts", Description = "Old", MaxDevs = 2 };
        var model = new UpdateProjectModel { Id = 1, Title = "The New Avengers", Description = "New", MaxDevs = 4 };

        _projectRepoMock.SetupSequence(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(existing);
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(false);

        await _sut.UpdateProjectAsync(model);

        existing.Title.Should().Be("The New Avengers");
        existing.Description.Should().Be("New");
        existing.MaxDevs.Should().Be(4);
        _projectRepoMock.Verify(r => r.Update(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenTitleTakenByAnotherProject_ShouldThrowConflictException()
    {
        var existing = new Project { IdProject = 1, Title = "Ratatouille" };
        var model = new UpdateProjectModel { Id = 1, Title = "Ratatin", Description = "Desc", MaxDevs = 2 };

        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(existing);
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(true);

        var act = async () => await _sut.UpdateProjectAsync(model);

        await act.Should().ThrowAsync<ConflictException>()
                 .WithMessage("Another project with the same title already exists.");
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenKeepingSameTitle_ShouldNotThrow()
    {
        var existing = new Project { IdProject = 1, Title = "Johnny McJohnny", Description = "Old", MaxDevs = 2 };
        var model = new UpdateProjectModel { Id = 1, Title = "Johnny McJohnny", Description = "New desc", MaxDevs = 3 };

        _projectRepoMock.Setup(r => r.GetFirstOrDefault(
                It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(existing);
        _projectRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Project, bool>>>()))
                        .ReturnsAsync(true);

        var act = async () => await _sut.UpdateProjectAsync(model);

        await act.Should().NotThrowAsync();
        _projectRepoMock.Verify(r => r.Update(existing), Times.Once);
    }
}
