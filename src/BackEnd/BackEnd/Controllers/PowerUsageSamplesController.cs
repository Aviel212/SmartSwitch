using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Models.Dto;
using AutoMapper;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerUsageSamplesController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;
        private readonly IMapper _mapper;

        public PowerUsageSamplesController(SmartSwitchDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/PowerUsageSamples/plug/DC:DD:C2:23:D6:60?amount=20
        [HttpGet("plug/{mac}")]
        public async Task<ActionResult<IEnumerable<PowerUsageSampleDto>>> GetPowerUsageSamples(string mac, [FromQuery]int amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Plug plug = await _context.Plugs.Include(p => p.Samples).SingleOrDefaultAsync(p => p.Mac == mac);
            if (plug == null) return NotFound();

            return Ok(_mapper.Map<List<PowerUsageSampleDto>>(plug.Samples.OrderByDescending(x => x.SampleDate).Take(amount)));
        }
    }
}