using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classrooms
{
    public class IndexModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public IndexModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        public IList<Classroom> Classroom { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Classrooms != null)
            {
                Classroom = await _context.Classrooms.Include(c=>c.Subject).ToListAsync();
            }
        }
    }
}
