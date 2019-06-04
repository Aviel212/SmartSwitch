using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using AutoMapper;
using BackEnd.Models.Dto;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlugsController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;
        private readonly IMapper _mapper;

        public PlugsController(SmartSwitchDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Plugs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlugDto>> GetPlug([FromRoute] string id)
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

            return Ok(_mapper.Map<PlugDto>(plug));
        }

        // GET: api/Plugs/User/{user.id}
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<PlugDto>>> GetUserPlugs([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _context.Users.Include(u => u.Plugs).SingleOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();


            return Ok(_mapper.Map<List<PlugDto>>(user.Plugs));
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