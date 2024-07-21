using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleService.Logic.Interfaces;
using ScheduleService.Models;
using ScheduleService.Service.Interfaces;

namespace ScheduleWeb.Pages.Timetables.Import
{
    public class FromFileModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITimetableImporter _importer;
        private readonly IInputService _inputService;

        public ImportResult<TimetableExtend> ImportResult { get; private set; } = default!;
        public List<TimetableDto> TimetableDtos { get; private set; } = default!;

        public FromFileModel(IWebHostEnvironment hostingEnvironment, ITimetableImporter importer, IInputService inputService)
        {
            _hostingEnvironment = hostingEnvironment;
            _importer = importer;
            _inputService = inputService;
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
                ImportResult = await _importer.ImportNewScheduleFromFileAsync(filePath);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while importing the schedule.");
                return Page();
            }
        }
    }
}
