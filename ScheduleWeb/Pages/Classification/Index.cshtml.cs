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
    public class IndexModel : PageModel
    {
        private readonly StudentManagementContext _context;

        public IndexModel(StudentManagementContext context)
        {
            _context = context;
        }

        public IList<StudentClassroom> StudentClassroom { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.StudentClassrooms != null)
            {
                StudentClassroom = await _context.StudentClassrooms
                .Include(s => s.Classroom)
                .Include(s => s.Student).ToListAsync();
            }
        }
    }
}
