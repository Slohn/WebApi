using WebApi.Models;
using WebApi.Contracts;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;

namespace WebApi.Data
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool Validation(LoginDTO model) 
        {
            var res = _appDbContext.Users.Where(item => item.Email == model.Email).FirstOrDefault();
            if (res != null) 
            {
                if (res.Password == Helpers.GetPasswordHash(model.Password)) 
                {
                    return true;
                }
            }
            return false;
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                if (user != null)
                {
                    var obj = _appDbContext.Add<User>(user);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(User user)
        {
            try
            {
                if (user != null)
                {
                    _appDbContext.Remove(user);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                var obj = await _appDbContext.Users.ToListAsync();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var obj = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
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

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                if (user != null)
                {
                    var obj = _appDbContext.Update<User>(user);
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
    }
}
