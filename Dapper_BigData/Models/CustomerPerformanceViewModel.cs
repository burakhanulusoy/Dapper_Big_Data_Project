namespace Dapper_BigData.Models
{
    public class CustomerPerformanceViewModel
    {
        public int Month { get; set; }
        public int MaxQuantity { get; set; }
        public string MaxCustomerName { get; set; }
        public int MinQuantity { get; set; }
        public string MinCustomerName { get; set; }
    }
}
