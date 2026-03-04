using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.BoardColumn;
using SimpleKanbanBoards.Business.Service;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Tests.Unit.Services
{
    public class BoardColumnServiceTests
    {
        private readonly Mock<IBoardColumnRepository> _boardColumnRepoMock;
        private readonly Mock<IBoardRepository> _boardRepoMock;
        private readonly BoardColumnService _sut;

        public BoardColumnServiceTests()
        {
            _boardColumnRepoMock = new Mock<IBoardColumnRepository>();
            _boardRepoMock = new Mock<IBoardRepository>();
            _sut = new BoardColumnService(_boardColumnRepoMock.Object, _boardRepoMock.Object);
        }

        [Fact]
        public async Task CreateBoardColumnAsync_WhenNoConflicts_ShouldAddBoardColumn()
        {
            var newColumn = new CreateBoardColumnModel
            {
                Name = "To Do",
                Position = 1,
                WipLimit = 5,
                IsEntry = true,
                IsDone = false,
                IdBoard = 10
            };

            _boardRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Board, bool>>>()))
                         .ReturnsAsync(true);
            _boardColumnRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(false);

            await _sut.CreateBoardColumnAsync(newColumn);

            _boardColumnRepoMock.Verify(r => r.AddAsync(It.Is<BoardColumn>(bc =>
                bc.BoardColumnName == newColumn.Name &&
                bc.ColumnPosition == newColumn.Position &&
                bc.WipLimit == newColumn.WipLimit &&
                bc.IsEntry == newColumn.IsEntry &&
                bc.IsDone == newColumn.IsDone &&
                bc.IdBoard == newColumn.IdBoard
            )), Times.Once);
        }

        [Fact]
        public async Task CreateBoardColumnAsync_WhenConflictExists_ShouldThrowConflictException()
        {
            var newColumn = new CreateBoardColumnModel
            {
                Name = "To Do",
                Position = 1,
                WipLimit = 5,
                IsEntry = true,
                IsDone = false,
                IdBoard = 10
            };
            _boardRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Board, bool>>>()))
                         .ReturnsAsync(true);
            _boardColumnRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(true);

            var act = async () => await _sut.CreateBoardColumnAsync(newColumn);

            await act.Should().ThrowAsync<ConflictException>();
            _boardColumnRepoMock.Verify(r => r.AddAsync(It.IsAny<BoardColumn>()), Times.Never);
        }

        [Fact]
        public async Task CreateBoardColumnAsync_WhenBoardDoesNotExist_ShouldThrowNotFoundException()
        {
            var newColumn = new CreateBoardColumnModel
            {
                Name = "To Do",
                Position = 1,
                WipLimit = 5,
                IsEntry = true,
                IsDone = false,
                IdBoard = 10
            };
            _boardRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Board, bool>>>()))
                         .ReturnsAsync(false);

            var act = async () => await _sut.CreateBoardColumnAsync(newColumn);

            await act.Should().ThrowAsync<NotFoundException>();
            _boardColumnRepoMock.Verify(r => r.AddAsync(It.IsAny<BoardColumn>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBoardColumnAsync_WhenColumnExists_ShouldRemoveBoardColumn()
        {
            var columnId = 33;
            var existingColumn = new BoardColumn { IdBoardColumn = columnId };
            _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(existingColumn);

            await _sut.DeleteBoardColumnAsync(columnId);

            _boardColumnRepoMock.Verify(r => r.Remove(existingColumn), Times.Once);
        }

        [Fact]
        public async Task DeleteBoardColumnAsync_WhenColumnDoesNotExist_ShouldThrowNotFoundException()
        {
            var columnId = 33;
            _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync((BoardColumn)null);

            var act = async () => await _sut.DeleteBoardColumnAsync(columnId);

            await act.Should().ThrowAsync<NotFoundException>();
            _boardColumnRepoMock.Verify(r => r.Remove(It.IsAny<BoardColumn>()), Times.Never);
        }

        [Fact]
        public async Task GetBoardColumnByIdAsync_WhenColumnExists_ShouldReturnBoardColumnModel()
        {
            var boardId = 10;
            var existingColumn = new BoardColumn
            {
                IdBoardColumn = 5,
                BoardColumnName = "To Do",
                ColumnPosition = 1,
                WipLimit = 5,
                IsEntry = true,
                IsDone = false,
                IdBoard = boardId
            };

            _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(existingColumn);

            var result = await _sut.GetBoardColumnByIdAsync(boardId);

            result.Should().NotBeNull();
            result.Id.Should().Be(existingColumn.IdBoardColumn);
            result.Name.Should().Be(existingColumn.BoardColumnName);
            result.Position.Should().Be(existingColumn.ColumnPosition);
            result.WipLimit.Should().Be(existingColumn.WipLimit);
            result.IsEntry.Should().Be(existingColumn.IsEntry ?? false);
            result.IsDone.Should().Be(existingColumn.IsDone ?? false);
        }

        [Fact]
        public async Task GetBoardColumnByIdAsync_WhenColumnDoesNotExist_ShouldThrowNotFoundException()
        {
            var boardId = 10;
            _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync((BoardColumn)null);

            var act = async () => await _sut.GetBoardColumnByIdAsync(boardId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateBoardColumnAsync_WhenConflictExists_ShouldThrowConflictException()
        {
            var updateModel = new UpdateBoardColumnModel
            {
                Id = 5,
                Name = "In Progress",
                Position = 2,
                WipLimit = 3,
                IsEntry = false,
                IsDone = false
            };

            _boardColumnRepoMock.Setup(r => r.GetFirstOrDefault(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(new BoardColumn { IdBoardColumn = updateModel.Id });
            _boardColumnRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<BoardColumn, bool>>>()))
                                .ReturnsAsync(true);
            _boardRepoMock.Setup(r => r.Exist(It.IsAny<Expression<Func<Board, bool>>>()))
                         .ReturnsAsync(true);

            var act = async () => await _sut.UpdateBoardColumnAsync(updateModel);

            await act.Should().ThrowAsync<ConflictException>();
            _boardColumnRepoMock.Verify(r => r.Update(It.IsAny<BoardColumn>()), Times.Never);
        }
    }
}
