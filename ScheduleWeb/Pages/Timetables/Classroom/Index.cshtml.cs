using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScheduleCore.Entities;
using ScheduleWeb.Api;
using ScheduleWeb.Services;

namespace ScheduleWeb.Pages.Timetables
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedWeek { get; set; } = $"{DateTime.Today.Year}-W{CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)}";

        [BindProperty(SupportsGet = true)]
        public DateTime FirstDayOfWeek { get; set; }

        public IList<Timetable> WeeklyTimetable { get; set; } = new List<Timetable>();

        [BindProperty(SupportsGet = true)]
        public int ClassroomId { get; set; }

        public SelectList SelectClassrooms { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            var Classrooms = await _apiService.GetAsync<List<ScheduleCore.Entities.Classroom>>(ApiUrl.API_CLASSROOM);

            SelectClassrooms = new SelectList(Classrooms, nameof(ScheduleCore.Entities.Classroom.Id), nameof(ScheduleCore.Entities.Classroom.Name));
            // Parse selected week to get year and week number
            var selectedYear = int.Parse(SelectedWeek.Substring(0, 4));
            var selectedWeekNumber = int.Parse(SelectedWeek.Substring(6));

            // Calculate the date of the first day of the selected week
            FirstDayOfWeek = CultureInfo.CurrentCulture.Calendar
                .AddWeeks(new DateTime(selectedYear, 1, 1), selectedWeekNumber - 1)
                .AddDays(-(int)new DateTime(selectedYear, 1, 1).DayOfWeek + 1);
            
            try {
                var uriBuilder = new UriBuilder(ApiUrl.API_GET_TIMETABLE_CLASSROOM);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["ClassroomId"] = ClassroomId.ToString();
                query["SelectedWeek"] = SelectedWeek;
                uriBuilder.Query = query.ToString();
                string urlWithParameters = uriBuilder.ToString();
                WeeklyTimetable = await _apiService.GetAsync<List<Timetable>>(urlWithParameters) ?? new();
            }
            catch (HttpRequestException httpRequestException)
            {
                ErrorMessage = $"Request error: {httpRequestException.Message}";
            }
            catch (JsonException jsonException)
            {
                ErrorMessage = $"Serialization error: {jsonException.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
        }
    }
}
