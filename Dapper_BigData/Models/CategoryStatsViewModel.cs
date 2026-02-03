namespace Dapper_BigData.Models
{
    public class CategoryStatsViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; } // O kategoride kaç ürün var
        public int OrderCount { get; set; }   // Kaç kere sipariş edilmiş
        public decimal TotalRevenue { get; set; } // Toplam Kazanç
    }
}
