using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlugsController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;

        public PlugsController(SmartSwitchDbContext context)
        {
            _context = context;
        }

        // GET: api/Plugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlug([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plug = await _context.Plugs.FindAsync(id);

            if (plug == null)
            {
                return NotFound();
            }

            return Ok(plug);
        }

        // GET: api/Plugs/User/{user.id}
        [HttpGet("user/{userName}")]
        //[Route("")]
        public async Task<ActionResult<IEnumerable<Plug>>> GetUserPlugs([FromRoute] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plugs = await _context.Users.Where(u => u.Username == userName).Select(u => u.Plugs).FirstOrDefaultAsync();

            if (plugs == null)
            {
                return NotFound();
            }

            return Ok(plugs);
            //return _context.Plugs.Include("Username").ToList();//.Where(p => p.Username);
            //var plugs2 =  _context.Plugs.Where(
            //    p => _context.Entry(p).CurrentValues["Username"].ToString() != userName)
            //                                                                    .ToList();
            //var plugs = _context.Plugs.ToList();


            ////return Ok(plugs);
            //return plugs2;
        }

        // PUT: api/Plugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlug([FromRoute] string id, [FromBody] Plug plug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plug.Mac)
            {
                return BadRequest();
            }

            UpdatePowerState(id, plug.IsOn);

            _context.Entry(plug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlugExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Plugs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlug([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plug = await _context.Plugs.FindAsync(id);
            if (plug == null)
            {
                return NotFound();
            }

            _context.Plugs.Remove(plug);
            await _context.SaveChangesAsync();

            return Ok(plug);
        }

        private bool PlugExists(string id)
        {
            return _context.Plugs.Any(e => e.Mac == id);
        }

        private async void UpdatePowerState(string mac, bool newStatus)
        {
            Plug plug = await _context.Plugs.FindAsync(mac);
            if(plug == null)
            {
                return;             // exception
            }

            if (plug.IsOn != newStatus)
            {
                if (newStatus == true)
                    plug.TurnOn();
                else
                    plug.TurnOff();
            }
        }
    }
}