namespace Dapper_BigData.Models
{
    public class MonthlyProductStatsViewModel
    {
        public int Month { get; set; }
        public int UniqueProductCount { get; set; } // O ay kaç farklı ürün satıldı
        public string TopPaymentMethod { get; set; }

    }
}
