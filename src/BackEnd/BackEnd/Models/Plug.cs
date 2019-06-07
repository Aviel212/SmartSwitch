using Autofac;
using BackEnd.Models.Websockets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BackEnd.Models
{
    /// <summary>
    /// this class represents a smart device which you can turn on and off and use simple tasks on it
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
        public DateTime AddedAt { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<PowerUsageSample> Samples { get; set; }

        public Plug()
        {
            Tasks = new List<Task>();
            Samples = new List<PowerUsageSample>();
        }

        public Plug(string mac)
        {
            Mac = mac;
            IsOn = false;
            Approved = false;
            Priority = Priorities.IRRELEVANT;
            AddedAt = DateTime.Now;
            Tasks = new List<Task>();
            Samples = new List<PowerUsageSample>();
        }

        // turn the device on
        public async void TurnOn()
        {
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                if (await scope.Resolve<IWebsocketsServer>().TurnOn(Mac))
                {
                    IsOn = true;
                    SmartSwitchDbContext context = scope.Resolve<SmartSwitchDbContext>();
                    context.Entry(this).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
        }

        // turn the device off
        public async void TurnOff()
        {
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                try
                {
                    if (await scope.Resolve<IWebsocketsServer>().TurnOff(Mac))
                    {
                        IsOn = false;
                        SmartSwitchDbContext context = scope.Resolve<SmartSwitchDbContext>();
                        context.Entry(this).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                }
                catch (PlugNotConnectedException) { }
            }
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
            task.Schedule();
        }
    }
}