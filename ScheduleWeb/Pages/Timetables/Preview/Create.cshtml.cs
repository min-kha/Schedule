using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables.Preview
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
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name");
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Timetable Timetable { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Timetables == null || Timetable == null)
            {
                return Page();
            }

            _context.Timetables.Add(Timetable);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
