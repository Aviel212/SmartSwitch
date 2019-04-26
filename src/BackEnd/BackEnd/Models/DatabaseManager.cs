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
    }
}