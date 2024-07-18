using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using ScheduleCore.Entities;
using ScheduleService.Logic;
using ScheduleService.Logic.Interfaces;

namespace ScheduleWeb.Pages.Timetables
{
    public class CreateModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;
        private readonly ITimetableService _timetableService;

        public CreateModel(ScheduleCore.Entities.StudentManagementContext context, ITimetableService timetableService)
        {
            _context = context;
            _timetableService = timetableService;
        }

        public IActionResult OnGet()
        {
            GetData();
            return Page();
        }

        private void GetData()
        {
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "Id", "Code");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", nameof(Room.RoomNumber));
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Code");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", nameof(Teacher.TeacherCode));
        }

        [BindProperty]
        public Timetable Timetable { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var errorMes = await _timetableService.CheckTimetableConflictAsync(Timetable);
            if (!ModelState.IsValid || _context.Timetables == null || Timetable == null || errorMes != null)
            {
                GetData();
                //ViewData["ErrorMessage"] = errorMes;
                if (errorMes != null)
                    ModelState.AddModelError(string.Empty, errorMes);
                return Page();
            }

            _context.Timetables.Add(Timetable);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm lịch thành công";
            GetData();
            return Page();
        }
    }
}
