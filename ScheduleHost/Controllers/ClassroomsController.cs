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
    public class ClassroomsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public ClassroomsController(StudentManagementContext context)
        {
            _context = context;
        }

        [HttpGet("subject")]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassroomsBySubject(int subjectId, int semester)
        {
            if (_context.Classrooms == null)
            {
                return NotFound();
            }
            return await _context.Classrooms.Include(c => c.Subject).Where(c=>c.SubjectId==subjectId && c.Semesters == semester).ToListAsync();
        }

        // GET: api/Classrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
          if (_context.Classrooms == null)
          {
              return NotFound();
          }
            return await _context.Classrooms.Include(c => c.Subject).ToListAsync();
        }

        // GET: api/Classrooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
          if (_context.Classrooms == null)
          {
              return NotFound();
          }
            var classroom =  _context.Classrooms.Include(c => c.Subject).FirstOrDefault(c=>c.Id==id);

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }

        // PUT: api/Classrooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassroom(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return BadRequest();
            }

            _context.Entry(classroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
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

        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom(Classroom classroom)
        {
          if (_context.Classrooms == null)
          {
              return Problem("Entity set 'StudentManagementContext.Classrooms'  is null.");
          }
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassroom", new { id = classroom.Id }, classroom);
        }

        // DELETE: api/Classrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            if (_context.Classrooms == null)
            {
                return NotFound();
            }
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassroomExists(int id)
        {
            return (_context.Classrooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
