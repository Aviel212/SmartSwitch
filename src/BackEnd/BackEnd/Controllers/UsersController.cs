using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;

        public UsersController(SmartSwitchDbContext context)
        {
            _context = context;
            DatabaseManager.GetInstance().Context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            List<User> allUsers = await _context.Users.ToListAsync();
            List<string> usernames = new List<string>();

            foreach (User u in allUsers)
            {
                usernames.Add(u.UserName);
            }
            return usernames;

            //List<Plug> allPlugs = await _context.Plugs.ToListAsync();
            //List<string> nicknames = new List<string>();
            //nicknames.Add(Convert.ToString(_context.Entry(_context.Plugs.First()).CurrentValues["UserName"]));
            //foreach (Plug p in allPlugs)
            //{
            //    nicknames.Add(Convert.ToString(p.Nickname));
            //}
            //return nicknames;
        }

        // GET: api/Users/yarden
        [HttpGet("{username}", Name = "Get")]
        public string Get(string username)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_context.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(username.ToLower())));
        }

        // POST: api/Users
        // example: api/Users/op/yarden/12345
        [HttpPost("{op}/{username}/{password}", Name = "Post")]
        public string Post(string op, string username, string password)
        {
            if (op.Equals("create"))
            {
                try
                {
                    _context.Users.Add(new User(username, password));
                    _context.SaveChangesAsync();
                    return "added " + username;
                }
                catch (UsernameAlreadyInUseException)
                {
                    return "username exists";
                }
            }
            return "op not recognized";
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
