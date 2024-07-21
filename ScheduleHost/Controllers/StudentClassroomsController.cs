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
    public class StudentClassroomsController : ControllerBase
    {
        private readonly StudentManagementContext _context;

        public StudentClassroomsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: api/StudentClassrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClassroom>>> GetStudentClassrooms()
        {
          if (_context.StudentClassrooms == null)
          {
              return NotFound();
          }
            return await _context.StudentClassrooms.ToListAsync();
        }

        // GET: api/StudentClassrooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentClassroom>> GetStudentClassroom(int id)
        {
          if (_context.StudentClassrooms == null)
          {
              return NotFound();
          }
            var studentClassroom = await _context.StudentClassrooms.FindAsync(id);

            if (studentClassroom == null)
            {
                return NotFound();
            }

            return studentClassroom;
        }

        // PUT: api/StudentClassrooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentClassroom(int id, StudentClassroom studentClassroom)
        {
            if (id != studentClassroom.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentClassroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassroomExists(id))
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

        // POST: api/StudentClassrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentClassroom>> PostStudentClassroom(StudentClassroom studentClassroom)
        {
          if (_context.StudentClassrooms == null)
          {
              return Problem("Entity set 'StudentManagementContext.StudentClassrooms'  is null.");
          }
            _context.StudentClassrooms.Add(studentClassroom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentClassroom", new { id = studentClassroom.Id }, studentClassroom);
        }

        // DELETE: api/StudentClassrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentClassroom(int id)
        {
            if (_context.StudentClassrooms == null)
            {
                return NotFound();
            }
            var studentClassroom = await _context.StudentClassrooms.FindAsync(id);
            if (studentClassroom == null)
            {
                return NotFound();
            }

            _context.StudentClassrooms.Remove(studentClassroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentClassroomExists(int id)
        {
            return (_context.StudentClassrooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
