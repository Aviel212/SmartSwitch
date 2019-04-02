using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{
    /// <summary>
    /// User can add new plugs to his acctive plugs, remove plugs and more
    /// </summary>
    public class User
    {
        public string UserName { get; set;  } //changed to get and set instead of ;
        private string Password { get; set; }
        public List<Plug> Plugs { get; set; }

        public User(string name, string pass)
        {
            UserName = name;
            Password = pass;
        }

        public void AddPlug(Plug p)
        {
            //add new plug 
        }

        public Lazy<Plug> GetUnapprovedPlugs()
        {
            //need to return all unapproved Plugs
            return null;
        }

        public void RemovePlug(Plug p)
        {
            //need to remove certain device
        }

        public Plug GetPlug(string mac)
        {
            //need to return certain plug
            return null;
        }
    }
}