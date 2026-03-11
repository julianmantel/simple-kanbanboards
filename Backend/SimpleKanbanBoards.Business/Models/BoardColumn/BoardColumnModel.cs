using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Task;

namespace SimpleKanbanBoards.Business.Models.BoardColumn
{
    public class BoardColumnModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public int WipLimit { get; set; }
        public bool IsEntry { get; set; }
        public bool IsDone { get; set; }
        public int IdBoard { get; set; }
        public IEnumerable<TaskModel> Tasks { get; set; } = Enumerable.Empty<TaskModel>();
    }
}
