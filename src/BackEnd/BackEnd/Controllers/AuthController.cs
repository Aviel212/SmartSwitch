using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private UserManager<User> UserManager;
        private UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<User> userManager)
        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
            this._userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Username);
            if (user != null && await UserManager.CheckPasswordAsync(user, model.Password))
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                    issuer: "http://oec.com",
                    audience: "http://oec.com",
                    expires: DateTime.UtcNow.AddHours(1),
                    expires: DateTime.Now.AddDays(2),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });

            }
            return Unauthorized();
        }

        // POST api/accounts
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Post()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = "aa@b.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "dudi"
            };
            await _userManager.CreateAsync(user, "Password@123");

            return Ok();// new OkObjectResult("Account created");
        }


    }
}