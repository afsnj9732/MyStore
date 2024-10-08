﻿namespace MyStore.Server.Models.Repository.Dtos.DataModels
{
    public class ProductDataModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public int StockQuantity { get; set; }
    }
}
