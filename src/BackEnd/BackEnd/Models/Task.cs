using Autofac;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// Task class to execute some action at a certain time
    /// </summary>
    public class Task
    {
        public enum Operations { TurnOn, TurnOff };
        public enum TaskTypes { OneTime, Repeated };

        public int TaskId { get; set; }
        public Operations Operation { get; set; }
        public string DeviceMac { get; set; }
        public TaskTypes TaskType { get; set; }
        public int RepeatEvery { get; set; }
        public DateTime StartDate { get; set; }

        public Task() { }

        public Task(Operations op)
        {
            Operation = op;
        }

        public static void Execute(Operations op, string mac)
        {
            // when entering this function we need to execute the task
            Plug device;
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope()) device = scope.Resolve<SmartSwitchDbContext>().Plugs.Find(mac);

            Execute(op, device);
        }

        public static void Execute(Operations op, Plug device)
        {
            switch (op)
            {
                case Operations.TurnOn:
                    device.TurnOn();
                    break;
                case Operations.TurnOff:
                    device.TurnOff();
                    break;
            }
        }

        public void Schedule()
        {
            switch (TaskType)
            {
                case TaskTypes.OneTime:
                    BackgroundJob.Schedule(() => Execute(Operation, DeviceMac), StartDate - DateTime.Now);
                    break;
                case TaskTypes.Repeated:
                    BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(Operation, DeviceMac, RepeatEvery), StartDate - DateTime.Now);
                    break;
            }
        }

        public static void ExecuteAndScheduleNextExecution(Operations operation, string mac, int repeatEvery)
        {
            Execute(operation, mac);
            BackgroundJob.Schedule(() => ExecuteAndScheduleNextExecution(operation, mac, repeatEvery), TimeSpan.FromMinutes(repeatEvery));
        }
    }
}