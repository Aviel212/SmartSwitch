using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// Task class for execute some action in certain time
    /// </summary>
    public abstract class Task
    {
        public enum Operations { TURNON, TURNOFF };

        public int TaskId { get; set; }
        public Operations Operation { get; set; }
        public string DeviceMac { get; set; }

        public Task() { }

        public Task(Operations op)
        {
            Operation = op;
        }

        public static void Execute(Operations op, string mac)
        {
            //when entering this function we need to execute the task
            Plug device = DatabaseManager.GetInstance().GetPlug(mac);
            switch (op)
            {
                case Operations.TURNON:
                    device.TurnOn();
                    break;
                case Operations.TURNOFF:
                    device.TurnOff();
                    break;
                default:
                    break;
            }
        }

        public abstract void Schedule();
    }
}