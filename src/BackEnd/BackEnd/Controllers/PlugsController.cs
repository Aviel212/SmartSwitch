using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlugsController : ControllerBase
    {
        private readonly string plugNotConnected = "plug not connected";
        private readonly string valueNotRecognized = "value not recognized";

        // GET: api/Plugs/5
        [HttpGet("{mac}", Name = "Get")]
        public string Get(string mac) => Newtonsoft.Json.JsonConvert.SerializeObject(DatabaseManager.GetInstance().GetPlug(mac));

        // POST: api/Plugs
        [HttpPost("{mac}/{property}/{value}", Name = "Post")]
        public string Post(string mac, string property, string value)
        {
            Plug plug = DatabaseManager.GetInstance().GetPlug(mac);
            if (plug == null) return "no such plug";
            if (property.ToLower().Equals("ison"))
            {
                if (value.ToLower().Equals("true"))
                {
                    try
                    {
                        DatabaseManager.GetInstance().GetPlug(mac).TurnOn();
                        return "turned on";
                    }
                    catch (PlugNotConnectedException)
                    {
                        return plugNotConnected;
                    }
                }
                else if (value.ToLower().Equals("false"))
                {
                    try
                    {
                        DatabaseManager.GetInstance().GetPlug(mac).TurnOff();
                        return "turned off";
                    }
                    catch (PlugNotConnectedException)
                    {
                        return plugNotConnected;
                    }
                }
            }
            else if (property.ToLower().Equals("approved"))
            {
                if (value.ToLower().Equals("true"))
                {
                    plug.Approved = true;
                }
                else if (value.ToLower().Equals("false"))
                {
                    DatabaseManager.GetInstance().Context.Plugs.Remove(plug);
                }
                else return valueNotRecognized;

                return "ok";
            }
            else if (property.ToLower().Equals("priority"))
            {
                if (value.ToLower().Equals("essential")) plug.Priority = Plug.Priorities.ESSENTIAL;
                else if (value.ToLower().Equals("nonessential")) plug.Priority = Plug.Priorities.NONESSENTIAL;
                else if (value.ToLower().Equals("irrelevant")) plug.Priority = Plug.Priorities.IRRELEVANT;
                else return valueNotRecognized;
            }
            return "property not recognized";
        }
    }
}
