using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Timetables
{
    public class RoomsModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public RoomsModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public DateTime? Date { get; set; }
        public List<Timetable> Timetables { get; set; } = default!;
        public List<Room> Rooms { get; set; } = default!;
        public List<Slot> Slots { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Rooms = await _context.Rooms.OrderBy(r => r.RoomNumber).ToListAsync();
            Slots = await _context.Slots.OrderBy(s => s.Name).ToListAsync();
            if (Date == null)
            {
                Date = DateTime.Now;
            }
            if (_context.Timetables != null)
            {
                Timetables = await _context.Timetables
                .Include(t => t.Classroom)
                .Include(t => t.Subject)
                .Include(t => t.Teacher).Where(t => t.Date.Date == (Date ?? DateTime.Now).Date).ToListAsync();
            }
        }
    }
}
