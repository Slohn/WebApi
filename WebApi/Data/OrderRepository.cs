using Microsoft.EntityFrameworkCore;
using WebApi.Common.Search;
using WebApi.Models;

namespace WebApi.Data
{
    public class OrderRepository
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateAsync(Order order)
        {
            try
            {
                if (order != null)
                {
                    var obj = _appDbContext.Add<Order>(order);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Order Order)
        {
            try
            {
                if (Order != null)
                {
                    _appDbContext.Remove(Order);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var obj = await _appDbContext.Orders.Include(item => item.Products).ToListAsync();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> GetByIdAsync(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var obj = await _appDbContext.Orders.Include(item => item.Products).FirstOrDefaultAsync(x => x.Id == Id);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> UpdateAsync(Order Order)
        {
            try
            {
                if (Order != null)
                {
                    var obj = _appDbContext.Update<Order>(Order);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrders(OrderSearchParams searchParams) 
        {
            IQueryable<Order> res = _appDbContext.Orders.Include(item => item.Products);
            if(searchParams.SrartDate != null) 
            {
                res = res.Where(item => item.Date > searchParams.SrartDate);
            }
            if (searchParams.EndDate != null)
            {
                res = res.Where(item => item.Date < searchParams.EndDate);
            }
            if (searchParams.ProductId != null)
            {
                res = res.Where(item => item.Products.Any(item => item.Id == searchParams.ProductId));
            }
            if (searchParams.UserId != null)
            {
                res = res.Where(item => item.UserId == searchParams.UserId );
            }
            return res.ToList();
        }


    }
}
