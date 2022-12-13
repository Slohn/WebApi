namespace WebApi.Models
{
    public class OrderHistoryModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public decimal TotalCost { get; set; }
        public int ProductsCount { get; set; }
    }
}
