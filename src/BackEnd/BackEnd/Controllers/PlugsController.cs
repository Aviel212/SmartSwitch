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
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> PutPlug([FromRoute] string id, [FromBody] PlugDtoIn plugDtoIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plugDtoIn.Mac)
            {
                return BadRequest();
            }

            Plug plug = await _context.Plugs.FindAsync(plugDtoIn.Mac);
            _mapper.Map(plugDtoIn, plug);

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

        // TODO - add IsDeleted?
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
    }
}