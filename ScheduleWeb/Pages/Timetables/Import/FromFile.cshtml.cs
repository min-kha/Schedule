using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleCore.Entities;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;
using ScheduleService.Service.Interfaces;
using ScheduleWeb.Api;
using ScheduleWeb.Services;
using System.Web;

namespace ScheduleWeb.Pages.Timetables.Import
{
    public class FromFileModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IInputService _inputService;
        private readonly ApiService _apiService;

        public ImportResult<TimetableExtend> ImportResult { get; private set; } = default!;
        public List<TimetableDto> TimetableDtos { get; private set; } = default!;

        public FromFileModel(IWebHostEnvironment hostingEnvironment, IInputService inputService, ApiService apiService)
        {
            _hostingEnvironment = hostingEnvironment;
            _inputService = inputService;
            _apiService = apiService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            const string UPLOAD_FOLDER = "uploads";
            IFormFile file = Request.Form.Files[0];
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, UPLOAD_FOLDER);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string filePath = await _inputService.CopyFileToHost(uploadsFolder, file);
            try
            {
                IEnumerable<TimetableDto> timetableDtos = await _inputService.ReadFromFileAsync(filePath);
                TimetableDtos = timetableDtos.ToList();

                ImportResult = await _apiService.PostAsync<ImportResult<TimetableExtend>>(ApiEnpoint.API_POST_SCHEDULE_FROM_FILE, filePath);
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
