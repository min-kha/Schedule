using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables
{
    public class IndexModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public IndexModel(ScheduleCore.Entities.ScheduleContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public string SelectedWeek { get; set; } = $"{DateTime.Today.Year}-W{CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)}"; //SelectedWeek=2024-W39
        [BindProperty(SupportsGet = true)]
        public DateTime FirstDayOfWeek { get; set; }

        public IList<Timetable> WeeklyTimetable { get; set; } = new List<Timetable>();
        [BindProperty(SupportsGet = true)]
        public int TeacherId { get; set; }
        public SelectList SelectTeachers { get; set; }
        public async Task OnGetAsync()
        {
            SelectTeachers = new SelectList(_context.Teachers, nameof(Teacher.Id), nameof(Teacher.Name));
            // Parse selected week to get year and week number
            var selectedYear = int.Parse(SelectedWeek.Substring(0, 4));
            var selectedWeekNumber = int.Parse(SelectedWeek.Substring(6));

            // Calculate the date of the first day of the selected week
            FirstDayOfWeek = CultureInfo.CurrentCulture.Calendar
                .AddWeeks(new DateTime(selectedYear, 1, 1), selectedWeekNumber - 1)
                .AddDays(-((int)new DateTime(selectedYear, 1, 1).DayOfWeek) + 1);
            var lastDayOfWeek = FirstDayOfWeek.AddDays(4);
            var timetablesInSelectedWeek = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => t.TeacherId == TeacherId && t.Date >= FirstDayOfWeek && t.Date <= lastDayOfWeek)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.SlotId)
                .ToListAsync();

            WeeklyTimetable = timetablesInSelectedWeek;
        }

    }
}
