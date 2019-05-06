using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        // POST: api/Tasks
        [HttpPost("{jsonString}", Name = "PostTask")]
        public string Post(string jsonString)
        {
            Console.WriteLine("ddd: " + jsonString);
            JObject json = JObject.Parse(jsonString);
            if (!json.ContainsKey("Mac")) return "missing mac";

            Plug plug = DatabaseManager.GetInstance().GetPlug(json["Mac"].ToString());
            Models.Task.Operations op = json["Operation"].ToString().Equals("TURNON") ? Models.Task.Operations.TURNON : Models.Task.Operations.TURNOFF;


            if (json.ContainsKey("RepeatEvery")) // RepeatedTask
            {
                plug.AddTask(new RepeatedTask(op, DateTime.Parse(json["StartDate"].ToString().Replace('x', '+')), (int) json["RepeatEvery"]));
            }
            else // OneTimeTask
            {
                plug.AddTask(new OneTimeTask(op, DateTime.Parse(json["DateToBeExecuted"].ToString().Replace('x', '+'))));
            }

            DatabaseManager.GetInstance().Context.SaveChanges();
            return "ok";
        }
    }
}
