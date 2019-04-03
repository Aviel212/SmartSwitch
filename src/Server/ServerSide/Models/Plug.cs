using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{
    /// <summary>
    /// this class representing a smart device which you can turn on and off and use simple tasks on it
    /// </summary>
    public class Plug
    {
        public enum Priorities { ESSENTIAL, NONESSENTIAL, IRRELEVANT };

        public readonly string Mac;
        public string Nickname;
        public bool IsOn;
        public bool Approved;
        public Priorities Priority;
        public List<Task> Tasks;

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