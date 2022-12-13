namespace WebApi.Models
{
    public class UserViewModel
    {
        public User User { get; set; }
        public int OrderCount { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
