using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.AspNetCore.Cors;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;

        public UsersController(SmartSwitchDbContext context) => _context = context;

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            User user = await _context.FindAsync<User>(username);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost("username")]
        public async Task<ActionResult> CreateUser(string _username, [FromBody] string password)
        {
            User newUser = new User(_username, password);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { _username }, newUser);
        }

        [HttpPut("username")]
        public async Task<ActionResult> UpdateUser(string username, User user)
        {
            if (username != user.Username) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
