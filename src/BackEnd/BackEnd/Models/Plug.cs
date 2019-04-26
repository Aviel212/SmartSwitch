using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<Task> Tasks { get; set; }

        public Plug() { }

        public Plug(string mac)
        {
            Mac = mac;
            IsOn = false;
            Approved = false;
            Priority = Priorities.IRRELEVANT;
        }

        public void TurnOn()
        {
            //turn on the device
        }

        public void TurnOff()
        {
            //turn off the device
        }

        public void AddSample(PowerUsageSample pus)
        {
            //add a new sample to the list of samples
        }
    }
}