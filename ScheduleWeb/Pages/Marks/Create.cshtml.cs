using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Marks
{
    public class CreateModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public CreateModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "Id", "Code");
        ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
            return Page();
        }

        [BindProperty]
        public Mark Mark { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Marks == null || Mark == null)
            {
                return Page();
            }

            _context.Marks.Add(Mark);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
