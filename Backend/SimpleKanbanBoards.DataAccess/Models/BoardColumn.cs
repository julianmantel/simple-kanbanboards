using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class BoardColumn
{
    public int IdBoardColumn { get; set; }

    public int? IdBoard { get; set; }

    public string? BoardColumnName { get; set; }

    public int? ColumnPosition { get; set; }

    public int? WipLimit { get; set; }

    public bool? IsEntry { get; set; }

    public bool? IsDone { get; set; }

    public virtual Board? IdBoardNavigation { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
