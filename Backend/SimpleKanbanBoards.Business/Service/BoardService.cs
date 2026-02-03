using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Service.IService;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.Business.Service
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task CreateBoardAsync(BoardModel board)
        {
            var newBoard = new Board
            {
                BoardName = board.Name,
                Description = board.Description,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true,
                IdProject = board.ProjectId
            };

            await _boardRepository.AddAsync(newBoard);
        }

        public async Task ToggleBoardAsync(int boardId)
        {
            var board = await _boardRepository.GetFirstOrDefault(b => b.IdBoard == boardId);
            if(board == null)
            {
                throw new Exception("Board not found.");
            }

            await _boardRepository.ToggleBoardAsync(board);
        }

        public async Task<BoardModel> GetBoardByIdAsync(int boardId)
        {
            var board = await _boardRepository.GetFirstOrDefault(b => b.IdBoard == boardId);
            if (board == null)
            {
                throw new Exception("Board not found.");
            }

            return new BoardModel
            {
                Id = board.IdBoard,
                Name = board.BoardName,
                Description = board.Description,
                Created_At = board.CreatedAt ?? DateOnly.FromDateTime(DateTime.Now),
                Is_Active = board.IsActive ?? false,
                ProjectId = board.IdProject ?? 0
            };
        }

        public async Task UpdateBoardAsync(UpdateBoardModel updateBoard)
        {
            var board = await _boardRepository.GetFirstOrDefault(b => b.IdBoard == updateBoard.Id);
            if (board == null)
            {
                throw new Exception("Board not found.");
            }

            board.BoardName = updateBoard.Name;
            board.Description = updateBoard.Description;
            _boardRepository.Update(board);
        }
    }
}
