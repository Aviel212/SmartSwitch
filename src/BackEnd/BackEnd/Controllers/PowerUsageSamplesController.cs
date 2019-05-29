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
    public class PowerUsageSamplesController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;

        public PowerUsageSamplesController(SmartSwitchDbContext context)
        {
            _context = context;
        }

        // GET: api/PowerUsageSamples/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPowerUsageSample([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var powerUsageSample = await _context.PowerUsageSamples.FindAsync(id);

            if (powerUsageSample == null)
            {
                return NotFound();
            }

            return Ok(powerUsageSample);
        }
    }
}