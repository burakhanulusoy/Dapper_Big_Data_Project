namespace Dapper_BigData.Models
{
    public class CategoryPriceStatsViewModel
    {
        public string CategoryName { get; set; }

        public decimal MaxPrice { get; set; }
        public string MaxProductName { get; set; } // En pahalı ürünün adı

        public decimal MinPrice { get; set; }
        public string MinProductName { get; set; } // En ucuz ürünün adı

        public decimal AvgPrice { get; set; }
    }
}
