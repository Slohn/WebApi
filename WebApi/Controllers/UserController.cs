using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Common;
using WebApi.Contracts;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository Repository;
        public UserController(UserRepository repository)
        {
            Repository = repository;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO model)
        {



            try
            {
                if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
                    return BadRequest("Username and/or Password not specified");
                var res = Repository.Validation(model);
                if (res != null)
                {
                    var claims = new List<Claim>() 
                    { 
                        new Claim("role", res.Role.ToString()),
                        new Claim("id" , res.Id.ToString())
                    };
                    var now = DateTime.UtcNow;
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: Models.AuthOptions.ISSUER,
                        audience: Models.AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: now.Add(TimeSpan.FromMinutes(Models.AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(Models.AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );
                    return Ok(new JwtSecurityTokenHandler().
                    WriteToken(jwtSecurityToken));
                }
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("reg")]
        public async Task<IActionResult> Registration(LoginDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password)) 
            {
                return BadRequest();
            }
            try
            {
                await Repository.CreateAsync(new User { Email = model.Email, Password = Helpers.GetPasswordHash(model.Password) });
            }
            catch (Exception e) 
            {
                throw;
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize(Roles = "Admin")]
        public async Task<UserViewModel> GetUserInfo(int userId) 
        {
            var res = await Repository.GetByIdAsync(userId);
            return new UserViewModel 
            {
                User = new User {Email = res.Email, Id = res.Id,Role = res.Role, Password = res.Password },
                OrderCount = res.Orders.Count(),
                OrderAmount = res.Orders.Sum(item => item.Products.Sum(item => item.Price)),
            };
        }
    }
}
