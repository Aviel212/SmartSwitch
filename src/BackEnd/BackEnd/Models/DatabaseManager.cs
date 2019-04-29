using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// A singleton class that manages the entire Database
    /// </summary>
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        public SmartSwitchDbContext Context { get; set; }

        public DatabaseManager()
        {

        }

        public static DatabaseManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseManager();
            }
            return _instance;
        }

        public User GetUser(string username) => Context.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(username.ToLower()));

        public Plug GetPlug(string mac) => Context.Plugs.FirstOrDefault(x => x.Mac == mac);
    }
}