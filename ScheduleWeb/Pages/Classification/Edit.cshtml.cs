using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classification
{
    public class EditModel : PageModel
    {
        private readonly StudentManagementContext _context;

        public EditModel(StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentClassroom StudentClassroom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudentClassrooms == null)
            {
                return NotFound();
            }

            var studentclassroom = await _context.StudentClassrooms.FirstOrDefaultAsync(m => m.Id == id);
            if (studentclassroom == null)
            {
                return NotFound();
            }
            StudentClassroom = studentclassroom;
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "Id", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
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

            _context.Attach(StudentClassroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassroomExists(StudentClassroom.Id))
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

        private bool StudentClassroomExists(int id)
        {
            return (_context.StudentClassrooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
