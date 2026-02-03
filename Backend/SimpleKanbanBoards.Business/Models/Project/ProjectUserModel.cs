using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.Business.Models.Project
{
    public class ProjectUserModel
    {
        public int IdProject { get; set; }
        public int IdDev { get; set; }

        public DateTime JoinAt { get; set; } = DateTime.Now;
    }
}
