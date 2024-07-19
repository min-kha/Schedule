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
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public DeleteModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Timetable Timetable { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Timetables == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetables.FirstOrDefaultAsync(m => m.Id == id);

            if (timetable == null)
            {
                return NotFound();
            }
            else 
            {
                Timetable = timetable;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Timetables == null)
            {
                return NotFound();
            }
            var timetable = await _context.Timetables.FindAsync(id);

            if (timetable != null)
            {
                Timetable = timetable;
                _context.Timetables.Remove(Timetable);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
