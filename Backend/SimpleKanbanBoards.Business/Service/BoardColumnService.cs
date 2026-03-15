using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Exceptions;
using SimpleKanbanBoards.Business.Models.BoardColumn;
using SimpleKanbanBoards.Business.Models.Task;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Business.Service
{
    public class BoardColumnService : IBoardColumnService
    {
        private readonly IBoardColumnRepository _boardColumnRepository;
        private readonly IBoardRepository _boardRepository;

        public BoardColumnService(IBoardColumnRepository boardColumnRepository, IBoardRepository boardRepository)
        {
            _boardColumnRepository = boardColumnRepository;
            _boardRepository = boardRepository;
        }

        public async Task CreateBoardColumnAsync(CreateBoardColumnModel boardColumn)
        {
            await CheckConflicts(boardColumn.IdBoard, boardColumn.Position, boardColumn.IsEntry, boardColumn.IsDone);

            var newBoardColumn = new BoardColumn
            {
                BoardColumnName = boardColumn.Name,
                ColumnPosition = boardColumn.Position,
                WipLimit = boardColumn.WipLimit,
                IsEntry = boardColumn.IsEntry,
                IsDone = boardColumn.IsDone,
                IdBoard = boardColumn.IdBoard
            };

            await _boardColumnRepository.AddAsync(newBoardColumn);
        }

        public async Task DeleteBoardColumnAsync(int columnId)
        {
            var boardColumn = await _boardColumnRepository.GetFirstOrDefault(bc => bc.IdBoardColumn == columnId);
            if (boardColumn == null)
            {
                throw new NotFoundException("Board column not found.");
            }

            _boardColumnRepository.Remove(boardColumn);
        }

        public async Task<BoardColumnModel> GetBoardColumnByIdAsync(int boardId)
        {
            var boardColumns = await _boardColumnRepository.GetFirstOrDefault(bc => bc.IdBoard == boardId);
            if (boardColumns == null)
            {
                throw new NotFoundException("Board column not found.");
            }

            return new BoardColumnModel
            {
                Id = boardColumns.IdBoardColumn,
                Name = boardColumns.BoardColumnName,
                Position = boardColumns.ColumnPosition ?? 0,
                WipLimit = boardColumns.WipLimit ?? 0,
                IsEntry = boardColumns.IsEntry ?? false,
                IsDone = boardColumns.IsDone ?? false,
                IdBoard = boardColumns.IdBoard ?? 0,
                Tasks = boardColumns.Tasks.Select(t => new TaskModel
                {
                    Id = t.IdTask,
                    IdUser = t.IdUser ?? 0,
                    IdBoardColumn = t.IdBoardColumn ?? 0,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt ?? DateTime.Now,
                    CompletedAt = t.CompletedAt,
                    Priority = t.Priority ?? 0,
                    ServiceClass = t.ServiceClass,
                }).ToList()
            };
        }

        public async Task<IEnumerable<BoardColumnModel>> GetBoardColumnsByBoardIdAsync(int boardId)
        {
            var boardColumns = await _boardColumnRepository.GetAll(bc => bc.IdBoard == boardId);
            if (boardColumns == null || !boardColumns.Any())
            {
                throw new NotFoundException("No board columns found for the specified board.");
            }

            return boardColumns.Select(bc => new BoardColumnModel
            {
                Id = bc.IdBoardColumn,
                Name = bc.BoardColumnName,
                Position = bc.ColumnPosition ?? 0,
                WipLimit = bc.WipLimit ?? 0,
                IsEntry = bc.IsEntry ?? false,
                IsDone = bc.IsDone ?? false,
                IdBoard = bc.IdBoard ?? 0,
                Tasks = bc.Tasks.Select(t => new TaskModel
                {
                    Id = t.IdTask,
                    IdUser = t.IdUser ?? 0,
                    IdBoardColumn = t.IdBoardColumn ?? 0,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt ?? DateTime.Now,
                    CompletedAt = t.CompletedAt,
                    Priority = t.Priority ?? 0,
                    ServiceClass = t.ServiceClass,
                }).ToList()
            }).ToList();
        }

        public async Task UpdateBoardColumnAsync(UpdateBoardColumnModel boardColumn)
        {
            var existingBoardColumn = await _boardColumnRepository.GetFirstOrDefault(bc => bc.IdBoardColumn == boardColumn.Id);
            if (existingBoardColumn == null)
            {
                throw new NotFoundException("Board column not found.");
            }

            await CheckConflicts(existingBoardColumn.IdBoard ?? 0, boardColumn.Position, boardColumn.IsEntry, boardColumn.IsDone);

            var updatedBoardColumn = new BoardColumn
            {
                IdBoardColumn = boardColumn.Id,
                BoardColumnName = boardColumn.Name,
                ColumnPosition = boardColumn.Position,
                WipLimit = boardColumn.WipLimit,
                IsEntry = boardColumn.IsEntry,
                IsDone = boardColumn.IsDone,
                IdBoard = existingBoardColumn.IdBoard
            };

            _boardColumnRepository.Update(updatedBoardColumn);
        }

        private async Task CheckConflicts(int idBoard, int columnPosition, bool isEntry, bool isDone)
        {
            var boardExists = await _boardRepository.Exist(b => b.IdBoard == idBoard);
            if (!boardExists)
            {
                throw new NotFoundException("Board not found.");
            }

            var hasPositionConflict = await _boardColumnRepository
                                                  .Exist(bc => bc.IdBoard == idBoard && bc.ColumnPosition == columnPosition);
            if (hasPositionConflict)
            {
                throw new ConflictException("A column with the same position already exists in the board.");
            }
            var existingEntryColumn = await _boardColumnRepository
                                                  .Exist(bc => bc.IdBoard == idBoard && bc.IsEntry.Value);
            if (isEntry && existingEntryColumn)
            {
                throw new ConflictException("An entry column already exists in the board.");
            }
            var existingDoneColumn = await _boardColumnRepository
                                                  .Exist(bc => bc.IdBoard == idBoard && bc.IsDone.Value);
            if (isDone && existingDoneColumn)
            {
                throw new ConflictException("A done column already exists in the board.");
            }
        }
    }
}
