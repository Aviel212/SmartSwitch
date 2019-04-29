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
        public virtual Plug Device { get; set; }

        public Task() { }

        public Task(Operations op, Plug dev)
        {
            Operation = op;
            Device = dev;
        }

        public void Execute()
        {
            //when entering this function we need to execute the task

            switch (Operation)
            {
                case Operations.TURNON:
                    Device.TurnOn();
                    break;
                case Operations.TURNOFF:
                    Device.TurnOff();
                    break;
                default:
                    break;
            }
        }
    }
}