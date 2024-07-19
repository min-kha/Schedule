using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Attends
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public DeleteModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Attend Attend { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Attends == null)
            {
                return NotFound();
            }

            var attend = await _context.Attends.FirstOrDefaultAsync(m => m.Id == id);

            if (attend == null)
            {
                return NotFound();
            }
            else 
            {
                Attend = attend;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Attends == null)
            {
                return NotFound();
            }
            var attend = await _context.Attends.FindAsync(id);

            if (attend != null)
            {
                Attend = attend;
                _context.Attends.Remove(Attend);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
