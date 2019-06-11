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
using BackEnd.Models.Auth;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SmartSwitchDbContext _smartSwitchDbContext;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        // POST api/Auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return BadRequest("no such user");

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateToken(user);

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });

            }
            return Unauthorized();
        }

        private JwtSecurityToken GenerateToken(ApplicationUser user)
        {
            var claims = new[]
{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

            var token = new JwtSecurityToken(
                issuer: null, // "http://oec.com", - Not required as no third-party is involved
                audience: null, //"http://oec.com", - Not required as no third-party is involved
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }  

        // POST api/Auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCheck = await _userManager.FindByNameAsync(model.Username);
            if (userCheck != null)
                return BadRequest(Error.UserAlreadyExists);

            // need to chek if user exists
            ApplicationUser user = new ApplicationUser()
            {
                //Email = "aa@b.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            IdentityResult createResult = await _userManager.CreateAsync(user, model.Password);       //password requird P @ 1????
            if (createResult.Succeeded)
            {
                _smartSwitchDbContext.Users.Add(new Models.User(model.Username, model.Password));
                return Ok(); // ObjectResult("Account created");
            }
            else
                return NotFound(createResult.Errors.ToString());
            
        }


    }
}