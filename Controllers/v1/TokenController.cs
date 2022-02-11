using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApi.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly ApplicationDbContext _context;
        public TokenController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AppUser appuser)
        {
            var adminExist = await _context.AppUsers.SingleOrDefaultAsync(a => a.UserName == "admin");
            if (adminExist==null)
            {
            AppUser admin = new AppUser()
                {
                    UserName = "admin",
                    Password = "123456"
                };
                await _context.AppUsers.AddAsync(admin);
                await _context.SaveChangesAsync();
            }
            
            //TODO: رمز رو عوض کنین اخر کار
            if (appuser !=null && appuser.UserName !=null && appuser.Password != null)
            {
                var user = await GetUser(appuser.UserName,appuser.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.UserId.ToString()),
                        new Claim("UserName",user.UserName),
                        new Claim("Password",user.Password),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires : DateTime.Now.AddDays(30),
                        signingCredentials : signIn);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("invalid crenedtial");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<AppUser> GetUser(string username,string password)
        {
            return await _context.AppUsers.SingleOrDefaultAsync(m => m.UserName == username && m.Password == password); 
        }
    }
}
