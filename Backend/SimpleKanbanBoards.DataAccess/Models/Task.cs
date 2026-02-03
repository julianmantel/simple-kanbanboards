using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public int? IdUser { get; set; }

    public int? IdBoardColumn { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? Priority { get; set; }

    public string? ServiceClass { get; set; }

    public virtual BoardColumn? IdBoardColumnNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
