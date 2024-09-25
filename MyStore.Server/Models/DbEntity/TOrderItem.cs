using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TOrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual TOrder Order { get; set; } = null!;

    public virtual TProduct Product { get; set; } = null!;
}
