using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.Task
{
    public class TaskModel
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        public int IdBoardColumn { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public int Priority { get; set; }

        public string? ServiceClass { get; set; }
    }
}
