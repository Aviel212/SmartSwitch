using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// User can add new plugs to his acctive plugs, remove plugs and more
    /// </summary>
    public class User
    {
        [Key]
        public string UserName { get; set;  }
        public string Password { get; set; }
        public List<Plug> Plugs { get; set; }

        public User() { }

        public User(string name, string pass)
        {
            foreach (User user in DatabaseManager.GetInstance().Context.Users)
            {
                if (user.UserName.Equals(name)) throw new UsernameAlreadyInUseException();
            }
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