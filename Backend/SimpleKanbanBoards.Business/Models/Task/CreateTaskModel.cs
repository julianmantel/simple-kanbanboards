using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.Task
{
    public class CreateTaskModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Priority { get; set; }
        public string? ServiceClass { get; set; }
        public int IdBoardColumn { get; set; }
    }
}
