using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Attends
{
    public class EditModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public EditModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Attend Attend { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Attends == null)
            {
                return NotFound();
            }

            var attend =  await _context.Attends.FirstOrDefaultAsync(m => m.Id == id);
            if (attend == null)
            {
                return NotFound();
            }
            Attend = attend;
           ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
           ViewData["TimeTableId"] = new SelectList(_context.Timetables, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Attend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendExists(Attend.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AttendExists(int id)
        {
          return (_context.Attends?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
