namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole 
    {
        User,
        Admin
    }
}
