using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Auth
{
    public class UserOwnershipValidator
    {
        public static bool IsNotValidated(string username, Plug plug, SmartSwitchDbContext context) => username != context.Entry(plug).CurrentValues["Username"].ToString();
        public static bool IsNotValidated(string username, User user) => username != user.Username;
    }
}
