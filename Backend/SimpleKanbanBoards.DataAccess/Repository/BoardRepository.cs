using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.DataAccess.Models;
using SimpleKanbanBoards.DataAccess.Repository.IRepository;
using Task = System.Threading.Tasks.Task;

namespace SimpleKanbanBoards.DataAccess.Repository
{
    public class BoardRepository(DbkanbanContext context) : RepositoryBase<Board>(context), IBoardRepository
    {
        public async Task ToggleBoardAsync(Board board)
        {
            board.IsActive = !board.IsActive;
            await _context.SaveChangesAsync();
        }
    }
}
