using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Marks
{
    public class EditModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public EditModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mark Mark { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark =  await _context.Marks.FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }
            Mark = mark;
           ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "Id", "Code");
           ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
           ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
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

            _context.Attach(Mark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkExists(Mark.Id))
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

        private bool MarkExists(int id)
        {
          return (_context.Marks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
