using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScheduleWeb.Pages.Timetables.DeleteMultiple
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public DateTime? StartTime { get; set; }

        [BindProperty]
        public DateTime? EndTime { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Confirm", new { startTime = StartTime, endTime = EndTime });
        }
    }
}
