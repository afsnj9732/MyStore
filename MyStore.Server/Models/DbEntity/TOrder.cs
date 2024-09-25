using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TOrder
{
    public int OrderId { get; set; }

    public int MemberId { get; set; }

    public int TotalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual TMember Member { get; set; } = null!;

    public virtual ICollection<TOrderItem> TOrderItems { get; set; } = new List<TOrderItem>();
}
