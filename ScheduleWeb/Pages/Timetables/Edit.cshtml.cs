using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables
{
    public class EditModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public EditModel(ScheduleCore.Entities.ScheduleContext context)
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

            var timetable =  await _context.Timetables.FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }
            Timetable = timetable;
           ViewData["ClassId"] = new SelectList(_context.Classrooms, "Id", "Code");
           ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
           ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name");
           ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
           ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Email");
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

            _context.Attach(Timetable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(Timetable.Id))
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

        private bool TimetableExists(int id)
        {
          return (_context.Timetables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
