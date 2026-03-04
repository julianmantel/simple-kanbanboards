using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.BoardColumn
{
    public class CreateBoardColumnModel
    {
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public int WipLimit { get; set; }
        public bool IsEntry { get; set; } = false;
        public bool IsDone { get; set; } = false;
        public int IdBoard { get; set; }
    }
}
