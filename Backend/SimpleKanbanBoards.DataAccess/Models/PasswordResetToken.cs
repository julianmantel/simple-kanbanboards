using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class PasswordResetToken
{
    public int IdResetToken { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpire { get; set; }

    public int? IdUser { get; set; }
}
