using System.ComponentModel;

namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        [DisplayName("商品名稱")]
        public string Name { get; set; } = null!;
        [DisplayName("商品單價")]
        public int Price { get; set; }
        [DisplayName("商品圖片")]
        public string? ImageUrl { get; set; }
        [DisplayName("商品介紹")]
        public string? Description { get; set; }
        [DisplayName("商品庫存")]
        public int StockQuantity { get; set; }
    }
}
