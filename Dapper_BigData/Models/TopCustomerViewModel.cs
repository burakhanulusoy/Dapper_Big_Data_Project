namespace Dapper_BigData.Models
{
    public class TopCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerImageUrl { get; set; }
        public string Email { get; set; }

        // YENİ EKLENENLER
        public string City { get; set; }
        public string Country { get; set; }

        public decimal TotalSpending { get; set; }
    }
}
