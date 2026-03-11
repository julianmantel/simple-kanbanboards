using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.Task
{
    public class UpdateTaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public int Priority { get; set; }
        public string? ServiceClass { get; set; }
        public int IdBoardColumn { get; set; }
    }
}
