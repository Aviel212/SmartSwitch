using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{
    /// <summary>
    /// A singelton class that manages the entire Database of users
    /// </summary>
    public class UserManager
    {
        private static UserManager _instance;
        public List<User> Users;

        public UserManager()
        {

        }

        public static UserManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserManager();
            }
            return _instance;
        }
    }
}