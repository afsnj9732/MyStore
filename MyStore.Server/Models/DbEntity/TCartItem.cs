using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TCartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual TCart Cart { get; set; } = null!;

    public virtual TProduct Product { get; set; } = null!;
}
