using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class Board
{
    public int IdBoard { get; set; }

    public int? IdProject { get; set; }

    public string? BoardName { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<BoardColumn> BoardColumns { get; set; } = new List<BoardColumn>();

    public virtual Project? IdProjectNavigation { get; set; }
}
