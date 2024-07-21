using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleCore.Entities;
using ScheduleWeb.Api;
using ScheduleWeb.Services;
using System.Diagnostics.Metrics;

namespace ScheduleWeb.Pages.Classification.Import
{
    public class RankModel : PageModel
    {
        private readonly ApiService _apiService;
        public List<Student> Students { get; private set; } = default!;
        [BindProperty]
        public IFormFile FileInput { get; set; }

        [BindProperty]
        public int Semester { get; set; }
        [BindProperty(SupportsGet = true)]
        public int SubjectId { get; set; }

        public SelectList SelectSubjects { get; set; }

        public List<Classroom> Classrooms{ get; set; }
        [BindProperty]
        public string? StringMessage { get; private set; }

        public RankModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task OnGetAsync()
        {
            var subjects = await _apiService.GetAsync<List<Subject>>(ApiUrl.API_SUBJECT);
            SelectSubjects = new SelectList(subjects, nameof(Subject.Id), nameof(Subject.Code));
        }

        // TODO: ApiService Post bị lỗi, code tạm
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        // Thêm file vào form data
                        if (FileInput != null)
                        {
                            var fileContent = new StreamContent(FileInput.OpenReadStream());
                            formData.Add(fileContent, "file", FileInput.FileName);
                        }

                        // Thêm các trường khác vào form data
                        formData.Add(new StringContent(Semester.ToString()), "semester");
                        formData.Add(new StringContent(SubjectId.ToString()), "subjectId");

                        // Gửi yêu cầu POST
                        var response = await httpClient.PostAsync(ApiUrl.API_POST_CLASSIFICATION_RANK_FROM_FILE, formData);

                        // Kiểm tra phản hồi
                        if (response.IsSuccessStatusCode)
                        {
                            StringMessage = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                            {
                                ModelState.AddModelError("", errorContent);
                            }
                            else
                            {
                                throw new Exception($"Lỗi server: {errorContent}");
                            }
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi không mong đợi: {ex.Message}");
                return Page();
            }
        }


    }
}
