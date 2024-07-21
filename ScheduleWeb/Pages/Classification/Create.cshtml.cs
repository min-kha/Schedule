using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classification
{
    public class CreateModel : PageModel
    {
        private readonly StudentManagementContext _context;

        public CreateModel(StudentManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "Id", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
            return Page();
        }

        [BindProperty]
        public StudentClassroom StudentClassroom { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.StudentClassrooms == null || StudentClassroom == null)
            {
                return Page();
            }

            _context.StudentClassrooms.Add(StudentClassroom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
