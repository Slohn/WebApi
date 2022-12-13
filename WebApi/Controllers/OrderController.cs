using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Search;
using WebApi.Data;
using WebApi.Models;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {


        private readonly OrderRepository Repository;

        public OrderController(OrderRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Order>> GerOrders(OrderSearchParams searchParams)
        {
            var res = Repository.GetAllAsync();
            return res.Result;
        }

        [HttpGet]
        [Route("GetUserOrders")]
        [Authorize]
        public async Task<IEnumerable<Order>> GetUserOrders(OrderSearchParams searchParams)
        {
            var res = Repository.GetOrders(searchParams);
            return res.Result;
        }

        [HttpGet]
        [Route("GetOrderById")]
        [Authorize]
        public async Task<Order> GetUserOrders(int orderId)
        {
            var res = Repository.GetByIdAsync(orderId);
            return res.Result;
        }

        [HttpGet]
        [Route("GetHistory")]
        [Authorize]
        public async Task<OrderHistoryModel> GetHistory() 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //if (identity != null)
            //{
            //    identity.FindFirst("id");

            //}
            var orders = await Repository.GetOrders(new OrderSearchParams { UserId = int.Parse(identity.FindFirst("id").Value)});

            return new OrderHistoryModel
            {
                Orders = orders,
                TotalCost = orders.Sum(item => item.Products.Sum(item => item.Price)),
                ProductsCount = orders.Sum(item => item.Products.Count)
            };
        }
    }
}
