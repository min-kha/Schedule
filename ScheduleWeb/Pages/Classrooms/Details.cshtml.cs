using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleWeb.Api;
using ScheduleWeb.Services;

namespace ScheduleWeb.Pages.Classrooms
{
    public class DetailsModel : PageModel
    {
        private readonly ApiService _apiService;

        public DetailsModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public List<Student> Student { get; set; } = default!;

        public Classroom Classroom { get; set; } = default!; 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _apiService.GetAsync<Classroom>(ApiUrl.API_CLASSROOM+id);
            if (classroom == null)
            {
                return NotFound();
            }
            else 
            {
                Classroom = classroom;
            }

            var uriBuilder = new UriBuilder(ApiUrl.API_GET_STUDENT_CLASSROOM);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["classroomId"] = id.ToString();
            uriBuilder.Query = query.ToString();
            string urlWithParameters = uriBuilder.ToString();
            var student = await _apiService.GetAsync<IEnumerable<Student>>(urlWithParameters); 
            Student = student?.ToList() ?? new List<Student>();
            return Page();
        }
    }
}
