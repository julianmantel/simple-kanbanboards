using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.BoardColumn;

namespace SimpleKanbanBoards.Business.Service.IService
{
    public interface IBoardColumnService
    {
        Task<BoardColumnModel> GetBoardColumnByIdAsync(int boardId);
        Task CreateBoardColumnAsync(CreateBoardColumnModel boardColumn);
        Task UpdateBoardColumnAsync(UpdateBoardColumnModel boardColumn);
        Task DeleteBoardColumnAsync(int columnId);
    }
}
