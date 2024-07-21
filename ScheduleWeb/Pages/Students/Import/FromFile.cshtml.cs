using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleCore.Entities;
using ScheduleWeb.Api;
using ScheduleWeb.Services;

namespace ScheduleWeb.Pages.Students.Import
{
    public class FromFileModel : PageModel
    {
        private readonly ApiService _apiService;
        public List<Student> Students { get; private set; } = default!;
        [BindProperty]
        public IFormFile FileInput { get; set; }

        public FromFileModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Students = await _apiService.PostFileAsync<List<Student>>(ApiUrl.API_POST_STUDENT_FROM_FILE, FileInput) ?? new();
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}
