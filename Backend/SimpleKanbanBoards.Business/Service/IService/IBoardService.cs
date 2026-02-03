using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Board;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IBoardService
    {
        Task CreateBoardAsync(BoardModel board);
        Task UpdateBoardAsync(UpdateBoardModel board);
        Task<BoardModel> GetBoardByIdAsync(int boardId);
        Task ToggleBoardAsync(int boardId);
    }
}
