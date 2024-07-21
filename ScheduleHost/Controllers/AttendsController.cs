using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public AttendsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: api/Attends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attend>>> GetAttends()
        {
          if (_context.Attends == null)
          {
              return NotFound();
          }
            return await _context.Attends.ToListAsync();
        }

        // GET: api/Attends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attend>> GetAttend(int id)
        {
          if (_context.Attends == null)
          {
              return NotFound();
          }
            var attend = await _context.Attends.FindAsync(id);

            if (attend == null)
            {
                return NotFound();
            }

            return attend;
        }

        // PUT: api/Attends/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttend(int id, Attend attend)
        {
            if (id != attend.Id)
            {
                return BadRequest();
            }

            _context.Entry(attend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendExists(id))
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

        // POST: api/Attends
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attend>> PostAttend(Attend attend)
        {
          if (_context.Attends == null)
          {
              return Problem("Entity set 'StudentManagementContext.Attends'  is null.");
          }
            _context.Attends.Add(attend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttend", new { id = attend.Id }, attend);
        }

        // DELETE: api/Attends/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttend(int id)
        {
            if (_context.Attends == null)
            {
                return NotFound();
            }
            var attend = await _context.Attends.FindAsync(id);
            if (attend == null)
            {
                return NotFound();
            }

            _context.Attends.Remove(attend);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendExists(int id)
        {
            return (_context.Attends?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
