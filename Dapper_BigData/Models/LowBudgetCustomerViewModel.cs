namespace Dapper_BigData.Models
{
    public class LowBudgetCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerImageUrl { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; } // Ödeme Yöntemi
        public string Status { get; set; }        // Sipariş Durumu
        public decimal TotalPrice { get; set; }   // Harcama Tutarı
    }
}
