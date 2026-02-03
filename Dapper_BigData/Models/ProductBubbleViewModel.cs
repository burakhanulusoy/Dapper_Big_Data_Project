namespace Dapper_BigData.Models
{
    public class ProductBubbleViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }        // X Ekseni
        public int TotalQuantity { get; set; }    // Y Ekseni
        public decimal TotalRevenue { get; set; } // Z Ekseni (Balon Boyutu)
    }
}
