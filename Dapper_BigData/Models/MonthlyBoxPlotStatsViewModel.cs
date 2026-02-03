namespace Dapper_BigData.Models
{
    public class MonthlyBoxPlotStatsViewModel
    {
        public int Month { get; set; }
        public decimal MinPrice { get; set; }
        public decimal Q1 { get; set; } // 1. Çeyrek (%25)
        public decimal Median { get; set; } // Ortanca (%50)
        public decimal Q3 { get; set; } // 3. Çeyrek (%75)
        public decimal MaxPrice { get; set; }
    }

    public class OutlierViewModel
    {
        public int Month { get; set; }
        public decimal Price { get; set; }
    }
}
