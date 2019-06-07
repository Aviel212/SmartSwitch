﻿using System;
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
    public class TasksController : ControllerBase
    {
        private readonly SmartSwitchDbContext _context;
        private readonly IMapper _mapper;

        public TasksController(SmartSwitchDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Tasks/plug/DC:DD:C2:23:D6:60
        [HttpGet("plug/{mac}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(string mac)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Plug plug = await _context.Plugs.Include(p => p.Tasks).SingleOrDefaultAsync(p => p.Mac == mac);
            if (plug == null) return NotFound();

            return Ok(_mapper.Map<List<TaskDto>>(plug.Tasks));
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody]TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Plug plug = await _context.Plugs.FindAsync(taskDto.DeviceMac);
            if (plug == null) return BadRequest();

            Models.Task task = _mapper.Map<Models.Task>(taskDto);
            plug.AddTask(task);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasks", new { mac = task.DeviceMac }, task);
        }

        // TODO
        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}