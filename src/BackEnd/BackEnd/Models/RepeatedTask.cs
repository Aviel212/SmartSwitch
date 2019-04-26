using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BackEnd.Models
{
    /// <summary>
    /// A task that repeats itself in a certain time
    /// </summary>
    public class RepeatedTask
    {
        public DateTime StartDate;
        public int RepeatEvery; // minutes

        public RepeatedTask()
        {

        }
    }
}