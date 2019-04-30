using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// this class representing a smart device which you can turn on and off and use simple tasks on it
    /// </summary>
    public class Plug
    {
        public enum Priorities { ESSENTIAL, NONESSENTIAL, IRRELEVANT };

        [Key]
        public string Mac { get; set; }
        public string Nickname { get; set; }
        public bool IsOn { get; set; }
        public bool Approved { get; set; }
        public Priorities Priority { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<PowerUsageSample> Samples { get; set; }

        public Plug() { }

        public Plug(string mac)
        {
            Mac = mac;
            IsOn = false;
            Approved = false;
            Priority = Priorities.IRRELEVANT;
            Tasks = new List<Task>();
            Samples = new List<PowerUsageSample>();
        }

        // turn the device on
        public void TurnOn()
        {
            if (WebsocketsServer.GetInstance().TurnOn(Mac))
            {
                IsOn = true;
                DatabaseManager.GetInstance().Context.SaveChanges();
            }
            
        }

        // turn the device off
        public void TurnOff()
        {
            if (WebsocketsServer.GetInstance().TurnOff(Mac))
            {
                IsOn = false;
                DatabaseManager.GetInstance().Context.SaveChanges();
            }
        }

        public void AddTask(Task task)
        {
            task.Device = this;
            Tasks.Add(task);
        }
    }
}