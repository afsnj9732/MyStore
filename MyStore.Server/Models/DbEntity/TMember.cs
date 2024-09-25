using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TMember
{
    public int MemberId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
