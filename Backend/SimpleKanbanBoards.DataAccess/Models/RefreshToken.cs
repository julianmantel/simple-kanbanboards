using System;
using System.Collections.Generic;

namespace SimpleKanbanBoards.DataAccess.Models;

public partial class RefreshToken
{
    public int IdRefreshToken { get; set; }

    public int? IdUser { get; set; }

    public string RefreshToken1 { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public int? ReplacedByToken { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<RefreshToken> InverseReplacedByTokenNavigation { get; set; } = new List<RefreshToken>();

    public virtual RefreshToken? ReplacedByTokenNavigation { get; set; }
}
