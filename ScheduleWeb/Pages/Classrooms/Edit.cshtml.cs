using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classrooms
{
    public class EditModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public EditModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Classroom Classroom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }

            var classroom =  await _context.Classrooms.FirstOrDefaultAsync(m => m.Id == id);
            if (classroom == null)
            {
                return NotFound();
            }
            Classroom = classroom;
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

            var classroomToUpdate = await _context.Classrooms.FindAsync(Classroom.Id);

            if (classroomToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Classroom>(
                classroomToUpdate,
                "Classroom",
                c => c.Name, c => c.Code, c => c.Semesters, c => c.Year, c => c.SubjectId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(Classroom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
            return Page();
        }


        private bool ClassroomExists(int id)
        {
          return (_context.Classrooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
