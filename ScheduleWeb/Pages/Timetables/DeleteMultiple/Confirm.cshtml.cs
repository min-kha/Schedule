using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using System.Linq;

namespace ScheduleWeb.Pages.Timetables.DeleteMultiple
{
    public class ConfirmModel : PageModel
    {
        private readonly ScheduleContext _context;

        public ConfirmModel(ScheduleContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet =true)]
        public DateTime? StartTime { get; set; } = DateTime.MinValue;
        [BindProperty(SupportsGet = true)]
        public DateTime? EndTime { get; set; } = DateTime.MaxValue;
        public IList<Timetable> Timetables { get; set; } = default!;
        public void OnGet(DateTime? startTime, DateTime? endTime)
        {
            Timetables = _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => t.Date.Date >= StartTime && t.Date.Date <= EndTime).ToList();
        }

        public IActionResult OnPost()
        {
            Timetables = _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Room)
                .Include(t => t.Slot)
                .Include(t => t.Subject)
                .Include(t => t.Teacher)
                .Where(t => t.Date.Date >= StartTime && t.Date.Date <= EndTime).ToList();
            if (Timetables!=null && Timetables.Any())
            {
                _context.Timetables.RemoveRange(Timetables);
                _context.SaveChanges();
            @TempData["SuccessMessage"] = "Xóa hàng loạt thành công";
            }
            return RedirectToPage("/Timetables/Rooms");
        }
    }
}
