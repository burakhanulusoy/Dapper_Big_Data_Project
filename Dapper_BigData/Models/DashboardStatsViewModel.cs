namespace Dapper_BigData.Models
{
    public class DashboardStatsViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int NotDeliveredOrders { get; set; }
    }
}
