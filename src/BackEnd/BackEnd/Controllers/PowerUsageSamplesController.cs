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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BackEnd.Models.Auth;

namespace BackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PowerUsageSamplesController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _currentUsername;

        public PowerUsageSamplesController(SmartSwitchDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _currentUsername = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            if (plug == null) return NotFound(Error.PlugDoesNotExist);

            if (UserOwnershipValidator.IsNotValidated(_currentUsername, plug, _context)) return Unauthorized(Error.UnauthorizedOwner);

            return Ok(_mapper.Map<List<PowerUsageSampleDto>>(plug.Samples.OrderByDescending(x => x.SampleDate).Take(amount)));
        }
    }
}