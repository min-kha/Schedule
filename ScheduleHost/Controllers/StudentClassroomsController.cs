using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleHost.DTOs;
using ScheduleService.Logic.Interfaces;

namespace ScheduleHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassroomsController : ControllerBase
    {
        private readonly StudentManagementContext _context;
        private readonly IInputService _inputService;

        public StudentClassroomsController(StudentManagementContext context, IInputService inputService)
        {
            _context = context;
            _inputService = inputService;
        }

        [HttpPost("import/rank")]
        public async Task<IActionResult> ImportStudents(IFormFile file, int semester, int subjectId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File chưa được chọn");
                }

                var studentRankDtos = (await _inputService.ReadFromFileAsync<StudentRankDto>(file)).ToList();

                if (!studentRankDtos.Any())
                {
                    return BadRequest("Không có bản ghi hợp lệ trong tệp");
                }

                var classrooms = await _context.Classrooms
                    .Where(c => c.SubjectId == subjectId && c.Semesters == semester)
                    .ToListAsync();

                if (!classrooms.Any())
                {
                    return BadRequest("Không tìm thấy lớp học phù hợp");
                }

                // Sắp xếp sinh viên theo Rank (giả sử Rank càng thấp càng giỏi)
                var sortedStudents = studentRankDtos.OrderBy(s => s.Rank).ToList();

                // Lấy danh sách sinh viên từ database dựa trên mã sinh viên
                var studentCodes = sortedStudents.Select(s => s.StudentCode).ToList();
                var students = await _context.Students
                    .Where(s => studentCodes.Contains(s.Code))
                    .ToDictionaryAsync(s => s.Code, s => s.Id);

                // Chia sinh viên vào các lớp
                var assignedStudents = new List<StudentClassroom>();
                for (int i = 0; i < sortedStudents.Count; i++)
                {
                    var studentCode = sortedStudents[i].StudentCode;
                    if (students.TryGetValue(studentCode, out var studentId))
                    {
                        var classroomIndex = i % classrooms.Count;
                        var classroom = classrooms[classroomIndex];

                        assignedStudents.Add(new StudentClassroom
                        {
                            StudentId = studentId,
                            ClassroomId = classroom.Id
                        });
                    }
                }

                // Lưu kết quả vào database
                await _context.StudentClassrooms.AddRangeAsync(assignedStudents);
                await _context.SaveChangesAsync();

                return Ok($"Đã phân bổ {assignedStudents.Count} sinh viên vào {classrooms.Count} lớp");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
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
