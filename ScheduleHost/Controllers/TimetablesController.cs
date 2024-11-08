﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleService.Models;
using ScheduleService.Service.Interfaces;

namespace ScheduleHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly StudentManagementContext _context;
        private readonly ITimetableImporter _importer;

        public TimetablesController(StudentManagementContext context, ITimetableImporter importer)
        {
            _context = context;
            _importer = importer;
        }

        [HttpPost("import")]
        public async Task<ActionResult<ImportResult<TimetableExtend>>> ImportSchedule([FromBody] string filePath)
        {
            try
            {
                ImportResult<TimetableExtend> importResult = await _importer.ImportNewScheduleFromFileAsync(filePath);
                return Ok(importResult);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while importing the schedule: {ex.Message}");
            }
        }

        [HttpGet("student")]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetablesForStudent(string studentCode, string selectedWeek)
        {
            var selectedYear = int.Parse(selectedWeek.Substring(0, 4));
            var selectedWeekNumber = int.Parse(selectedWeek.Substring(6));

            var firstDayOfWeek = CultureInfo.CurrentCulture.Calendar
                .AddWeeks(new DateTime(selectedYear, 1, 1), selectedWeekNumber - 1)
                .AddDays(-((int)new DateTime(selectedYear, 1, 1).DayOfWeek) + 1);
            var lastDayOfWeek = firstDayOfWeek.AddDays(4);
            // Find classroom by studentCode
            List<int> classroomIds = _context.StudentClassrooms.Where(sc => sc.Student.Code.ToUpper() == studentCode.ToUpper() && sc.Classroom != null).Select(sc=>sc.ClassroomId).ToList();
            var timetablesInSelectedWeek = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => classroomIds.Contains(t.ClassroomId??-1) && t.Date >= firstDayOfWeek && t.Date <= lastDayOfWeek)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.SlotId)
                .ToListAsync();

            return timetablesInSelectedWeek;
        }

        [HttpGet("classroom")]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetablesForClassroom(int classroomId, string selectedWeek)
        {
            var selectedYear = int.Parse(selectedWeek.Substring(0, 4));
            var selectedWeekNumber = int.Parse(selectedWeek.Substring(6));

            var firstDayOfWeek = CultureInfo.CurrentCulture.Calendar
                .AddWeeks(new DateTime(selectedYear, 1, 1), selectedWeekNumber - 1)
                .AddDays(-((int)new DateTime(selectedYear, 1, 1).DayOfWeek) + 1);
            var lastDayOfWeek = firstDayOfWeek.AddDays(4);

            var timetablesInSelectedWeek = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => t.ClassroomId == classroomId && t.Date >= firstDayOfWeek && t.Date <= lastDayOfWeek)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.SlotId)
                .ToListAsync();

            return timetablesInSelectedWeek;
        }

        [HttpGet("teacher")]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetablesForTeacher(int teacherId, string selectedWeek)
        {
            var selectedYear = int.Parse(selectedWeek.Substring(0, 4));
            var selectedWeekNumber = int.Parse(selectedWeek.Substring(6));

            var firstDayOfWeek = CultureInfo.CurrentCulture.Calendar
                .AddWeeks(new DateTime(selectedYear, 1, 1), selectedWeekNumber - 1)
                .AddDays(-((int)new DateTime(selectedYear, 1, 1).DayOfWeek) + 1);
            var lastDayOfWeek = firstDayOfWeek.AddDays(4);

            var timetablesInSelectedWeek = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => t.TeacherId == teacherId && t.Date >= firstDayOfWeek && t.Date <= lastDayOfWeek)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.SlotId)
                .ToListAsync();

            return timetablesInSelectedWeek;
        }

        // GET: api/Timetables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetables()
        {
          if (_context.Timetables == null)
          {
              return NotFound();
          }
            return await _context.Timetables.ToListAsync();
        }

        // GET: api/Timetables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timetable>> GetTimetable(int id)
        {
          if (_context.Timetables == null)
          {
              return NotFound();
          }
            var timetable = await _context.Timetables.FindAsync(id);

            if (timetable == null)
            {
                return NotFound();
            }

            return timetable;
        }

        // PUT: api/Timetables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable(int id, Timetable timetable)
        {
            if (id != timetable.Id)
            {
                return BadRequest();
            }

            _context.Entry(timetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(id))
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

        // POST: api/Timetables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
          if (_context.Timetables == null)
          {
              return Problem("Entity set 'StudentManagementContext.Timetables'  is null.");
          }
            _context.Timetables.Add(timetable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimetable", new { id = timetable.Id }, timetable);
        }

        // DELETE: api/Timetables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable(int id)
        {
            if (_context.Timetables == null)
            {
                return NotFound();
            }
            var timetable = await _context.Timetables.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }

            _context.Timetables.Remove(timetable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimetableExists(int id)
        {
            return (_context.Timetables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
