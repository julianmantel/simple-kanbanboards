using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.DataAccess.Models;

namespace SimpleKanbanBoards.Business.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? EndDate { get; set; }
        public int? MaxDevs { get; set; } = 0;
        public IEnumerable<BoardModel> Boards { get; set; } = Enumerable.Empty<BoardModel>();
    }
}
