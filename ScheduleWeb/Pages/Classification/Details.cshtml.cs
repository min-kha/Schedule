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
    public class DetailsModel : PageModel
    {
        private readonly StudentManagementContext _context;

        public DetailsModel(StudentManagementContext context)
        {
            _context = context;
        }

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
    }
}
