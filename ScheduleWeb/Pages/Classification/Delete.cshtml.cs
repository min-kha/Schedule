using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classification
{
    public class DeleteModel : PageModel
    {
        private readonly StudentManagementContext _context;

        public DeleteModel(StudentManagementContext context)
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
            else
            {
                StudentClassroom = studentclassroom;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.StudentClassrooms == null)
            {
                return NotFound();
            }
            var studentclassroom = await _context.StudentClassrooms.FindAsync(id);

            if (studentclassroom != null)
            {
                StudentClassroom = studentclassroom;
                _context.StudentClassrooms.Remove(StudentClassroom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
