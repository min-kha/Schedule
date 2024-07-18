using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public DeleteModel(ScheduleCore.Entities.ScheduleContext context)
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

            var timetable = await _context.Timetables
                .Include(t=>t.Classroom)
                .Include(t=>t.Room)
                .Include(t=>t.Slot)
                .Include(t=>t.Subject)
                .Include(t=>t.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

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
            TempData["SuccessMessage"] = "Xóa thành công";
            return RedirectToPage("./Rooms");
        }
    }
}
