using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        // POST: api/Tasks
        [HttpPost]
        public string Post([FromBody] string jsonString)
        {
            JObject json = JObject.Parse(jsonString);
            if (!json.ContainsKey("Mac")) return "missing mac";

            Plug plug = DatabaseManager.GetInstance().GetPlug(json["Mac"].ToString());
            Models.Task.Operations op = json["Operation"].ToString().Equals("TURNON") ? Models.Task.Operations.TURNON : Models.Task.Operations.TURNOFF;


            if (json.ContainsKey("RepeatEvery")) // RepeatedTask
            {
                plug.AddTask(new RepeatedTask(op, DateTime.Parse(json["StartDate"].ToString()), (int) json["RepeatEvery"]));
            }
            else // OneTimeTask
            {
                plug.AddTask(new OneTimeTask(op, DateTime.Parse(json["DateToBeExecuted"].ToString())));
            }

            DatabaseManager.GetInstance().Context.SaveChanges();
            return "ok";
        }
    }
}
