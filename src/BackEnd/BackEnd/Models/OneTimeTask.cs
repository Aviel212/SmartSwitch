using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BackEnd.Models
{
    /// <summary>
    /// A task that is un-repeatable (one-time-task)
    /// </summary>
    public class OneTimeTask
    {
        public DateTime DateToBeExecuted;

        public OneTimeTask()
        {

        }
    }
}