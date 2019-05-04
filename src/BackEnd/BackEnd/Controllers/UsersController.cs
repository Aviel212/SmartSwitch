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
        private readonly string noSuchUser = "no such user";

        public UsersController(SmartSwitchDbContext context)
        {
            _context = context;
            DatabaseManager.GetInstance().Context = context;
            Console.WriteLine("------------+++++++++init");
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> usernames = new List<string>();
            foreach (User u in _context.Users)
            {
                usernames.Add(u.UserName);
            }
            return usernames;
        }

        // GET: api/Users/yarden
        [HttpGet("{username}", Name = "Get")]
        public string Get(string username) => Newtonsoft.Json.JsonConvert.SerializeObject(DatabaseManager.GetInstance().GetUser(username));

        // POST: api/Users
        // example: api/Users/create/yarden/12345
        [HttpPost("{op}/{username}/{password}", Name = "Post")]
        public string Post(string op, string username, string password)
        {
            if (op.Equals("add"))
            {
                try
                {
                    _context.Users.Add(new User(username, password));
                    _context.SaveChanges();
                    return "added " + username;
                }
                catch (UsernameAlreadyInUseException)
                {
                    return "username exists";
                }
            }
            else if (op.Equals("remove"))
            {
                User toRemove = DatabaseManager.GetInstance().GetUser(username);
                if (toRemove == null) return noSuchUser;
                else
                {
                    if (!toRemove.Password.Equals(password)) return "wrong password";
                    _context.Users.Remove(toRemove);
                    _context.SaveChanges();
                    return "removed " + username;
                }
            }
            else if (op.Equals("change-password"))
            {
                User user = DatabaseManager.GetInstance().GetUser(username);
                if (user == null) return noSuchUser;
                else
                {
                    user.Password = password;
                    _context.SaveChanges();
                    return "password changed";
                }
            }
            return "op not recognized";
        }
    }
}
