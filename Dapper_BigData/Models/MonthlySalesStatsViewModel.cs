namespace Dapper_BigData.Models
{
    public class MonthlySalesStatsViewModel
    {
        public int Month { get; set; }
        public int TotalSales { get; set; }

        public int MaxSalesCount { get; set; }
        public string MaxProductName { get; set; }

        public int MinSalesCount { get; set; }
        public string MinProductName { get; set; }
    }
}
