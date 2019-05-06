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
        private readonly string noSuchUser = "no such user";

        // GET: api/Users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> usernames = new List<string>();
            foreach (User u in DatabaseManager.GetInstance().Context.Users)
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
                    DatabaseManager.GetInstance().Context.Users.Add(new User(username, password));
                    DatabaseManager.GetInstance().Context.SaveChanges();
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
                    DatabaseManager.GetInstance().Context.Users.Remove(toRemove);
                    DatabaseManager.GetInstance().Context.SaveChanges();
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
                    DatabaseManager.GetInstance().Context.SaveChanges();
                    return "password changed";
                }
            }
            return "op not recognized";
        }
    }
}
