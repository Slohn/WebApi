﻿namespace WebApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public List<Product> Products { get; set; } 
    }
}
