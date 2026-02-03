using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class UserProject
{
    public int IdUser { get; set; }

    public int IdProject { get; set; }

    public DateTime? JoinAt { get; set; }

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
