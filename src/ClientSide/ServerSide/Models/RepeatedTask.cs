using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ServerSide.Models
{
    /// <summary>
    /// A task that repeats itself in a certain time
    /// </summary>
    public class RepeatedTask
    {
        public DateTime StartDate;
        public int RepeatEvery;

        public RepeatedTask()
        {

        }
    }
}