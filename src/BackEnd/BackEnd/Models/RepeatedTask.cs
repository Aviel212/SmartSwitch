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

        public RepeatedTask(Operations op, DateTime startDate, int repeatEvery) : base(op)
        {
            StartDate = startDate;
            RepeatEvery = repeatEvery;
        }

        public static void ExecuteAndScheduleNextExecution(Operations operation, string mac, int repeatEvery)
        {
            Execute(operation, mac);
            BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(operation, mac, repeatEvery), TimeSpan.FromMinutes(repeatEvery));
        }

        public override void Schedule()
        {
            BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(Operation, DeviceMac, RepeatEvery), StartDate - DateTime.Now);
        }
    }
}