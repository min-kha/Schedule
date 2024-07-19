using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Attends
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
        ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Code");
        ViewData["TimeTableId"] = new SelectList(_context.Timetables, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Attend Attend { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Attends == null || Attend == null)
            {
                return Page();
            }

            _context.Attends.Add(Attend);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
