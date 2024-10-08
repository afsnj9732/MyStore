﻿namespace MyStore.Server.Models.Service.Dtos.Infos
{
    public class ProductInfo
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public int? Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public int? StockQuantity { get; set; }
    }
}
