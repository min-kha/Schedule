using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;
using ScheduleService.Models;

namespace ScheduleWeb.Pages.Classrooms
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public DeleteModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Classroom Classroom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms.FirstOrDefaultAsync(m => m.Id == id);

            if (classroom == null)
            {
                return NotFound();
            }
            else 
            {
                Classroom = classroom;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom != null)
            {
                Classroom = classroom;
                try
                {
                    _context.Classrooms.Remove(Classroom);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                    {
                            ModelState.AddModelError(string.Empty, "Lớp học đang được sử dụng, không thể xóa bây giờ.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi không mong muốn xảy ra.");
                    }
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
