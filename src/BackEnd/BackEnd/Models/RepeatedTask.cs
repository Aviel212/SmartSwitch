using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BackEnd.Models
{
    /// <summary>
    /// A task that repeats itself in a certain time
    /// </summary>
    public class RepeatedTask : Task
    {
        public DateTime StartDate;
        public int RepeatEvery; // minutes

        public RepeatedTask()
        {

        }

        public RepeatedTask(DateTime startDate, int repeatEvery)
        {
            StartDate = startDate;
            RepeatEvery = repeatEvery;
            BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(), StartDate - DateTime.Now);
        }

        private void ExecuteAndScheduleNextExecution()
        {
            Execute();
            BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(), TimeSpan.FromMinutes(RepeatEvery));
        }
    }
}