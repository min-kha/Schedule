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
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, Path.GetFileName(Request.Form.Files[0].FileName));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Request.Form.Files[0].CopyToAsync(fileStream);
            }

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
