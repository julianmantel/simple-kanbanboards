using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? MaxDevs { get; set; }

    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
}
