using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

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
        public async Task<IEnumerable<Order>> GerOrders()
        {
            var res = Repository.GetAllAsync();
            return res.Result;
        }
    }
}
