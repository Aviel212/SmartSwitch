using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Dto
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
