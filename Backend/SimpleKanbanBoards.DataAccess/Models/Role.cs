using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? RolName { get; set; }

    public virtual ICollection<User> IdUsers { get; set; } = new List<User>();
}
