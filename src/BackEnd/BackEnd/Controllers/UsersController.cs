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

        // GET: api/Users/plugs/{username}
        [HttpGet]
        [Route("plugs")]
        public IEnumerable<Plug> Get(string username)
        {
            //read plugs of yarden from db
            string uname = "yarden";
            User user = DatabaseManager.GetInstance().Context.Users.SingleOrDefault(x => x.UserName == uname);
            if (user == null)
                return null;
            List<Plug> Plugs = user.Plugs;
            //Plug p1 = new Plug("aa:bb")
            //{
            //    IsOn = true,
            //    Nickname = "TV",
            //    Approved = true,
            //    AddedAt = DateTime.Now,
            //    Priority = Plug.Priorities.ESSENTIAL
            //};
            //Plug p2 = new Plug("qq:ww")
            //{
            //    IsOn = false,
            //    Nickname = "Toaster",
            //    Approved = false,
            //    AddedAt = DateTime.Now,
            //    Priority = Plug.Priorities.NONESSENTIAL
            //};
            //Plug p3 = new Plug("yy:uu")
            //{
            //    IsOn = true,
            //    Nickname = "Fridge",
            //    Approved = true,
            //    AddedAt = new DateTime(2018, 01, 01),
            //    Priority = Plug.Priorities.ESSENTIAL
            //};
            //plugs.Add(p1);
            //plugs.Add(p2);
            //plugs.Add(p3);
            return Plugs;
        }


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
        [HttpGet("{username}/{password}", Name = "Get")]
        public string Get(string username, string password)
        {
            User user = DatabaseManager.GetInstance().GetUser(username);
            if (user == null) return noSuchUser;
            if (user.Password == password) return Newtonsoft.Json.JsonConvert.SerializeObject(user);
            else return "incorrect password";
        }

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
