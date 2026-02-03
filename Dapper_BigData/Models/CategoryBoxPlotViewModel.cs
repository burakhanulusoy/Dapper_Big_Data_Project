namespace Dapper_BigData.Models
{
    public class CategoryBoxPlotViewModel
    {
        public string CategoryName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal Q1 { get; set; }
        public decimal Median { get; set; }
        public decimal Q3 { get; set; }
        public decimal MaxPrice { get; set; }
    }
    public class CategoryOutlierViewModel
    {
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
    }
}
