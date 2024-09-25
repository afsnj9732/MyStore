using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TCart
{
    public int CartId { get; set; }

    public int MemberId { get; set; }

    public virtual TMember Member { get; set; } = null!;

    public virtual ICollection<TCartItem> TCartItems { get; set; } = new List<TCartItem>();
}
