using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class PlugAlreadyInUseException:Exception
    {
        public PlugAlreadyInUseException()
        {
        }

        public PlugAlreadyInUseException(string message)
            : base(message)
        {
        }

        public PlugAlreadyInUseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}