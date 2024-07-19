using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables.Preview
{
    public class IndexModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public IndexModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        public IList<Timetable> Timetable { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Timetables != null)
            {
                Timetable = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher).ToListAsync();
            }
        }
    }
}
