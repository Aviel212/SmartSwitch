﻿using System;
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
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual List<Plug> Plugs { get; set; }

        public User()
        {
            Plugs = new List<Plug>();
        }

        public User(string name, string pass)
        {
            if (DatabaseManager.GetInstance().GetUser(name) != null) throw new UsernameAlreadyInUseException();
            Username = name;
            Password = pass;
            Plugs = new List<Plug>();
        }

        public List<Plug> GetUnapprovedPlugs() => Plugs.Where(p => p.Approved == false).ToList();

        // return a certain plug
        public Plug GetPlug(string mac) => Plugs.FirstOrDefault(p => p.Mac == mac);
    }
}