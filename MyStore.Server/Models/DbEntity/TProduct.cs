using System;
using System.Collections.Generic;

namespace MyStore.Server.Models.DbEntity;

public partial class TProduct
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int StockQuantity { get; set; }

    public string StripePriceId { get; set; } = null!;

    public virtual ICollection<TCartItem> TCartItems { get; set; } = new List<TCartItem>();

    public virtual ICollection<TOrderItem> TOrderItems { get; set; } = new List<TOrderItem>();
}
