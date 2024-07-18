using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Teachers
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public DeleteModel(ScheduleCore.Entities.ScheduleContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }
            else 
            {
                Teacher = teacher;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher != null)
            {
                Teacher = teacher;
                try
                {
                    _context.Teachers.Remove(Teacher);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                    {
                        ModelState.AddModelError(string.Empty, "Giáo viên đã được xếp lịch dạy, không thể xóa bây giờ.");
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
