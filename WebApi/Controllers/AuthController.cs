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
    //[Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IRepository<User> Repository;
        public AuthController(IRepository<User> repository)
        {
            Repository = repository;
        }



        //public List<User> users = new List<User>()
        //    {
        //        new User {Email = "Baljit", Password = "dsddsd", Role = UserRole.User},
        //        new User {Email = "DDD", Password = "dsddsd", Role = UserRole.User}
        //    };

        [HttpGet]
        public IActionResult GetUsers() 
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login(LoginDTO model)
        {

            if (!ModelState.IsValid) 
            {
               return BadRequest("Username and/or Password not specified");
            }



            try
            {
                if (string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
                    return BadRequest("Username and/or Password not specified");
                if (model.Email.Equals("joydip") &&
                model.Password.Equals("joydip123"))
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                    var signinCredentials = new SigningCredentials
                   (secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        audience: "http://localhost:51398",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );
                    Ok(new JwtSecurityTokenHandler().
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
    }
}
