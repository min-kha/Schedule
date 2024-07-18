using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Subjects
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public DeleteModel(ScheduleCore.Entities.ScheduleContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Subject Subject { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == id);

            if (subject == null)
            {
                return NotFound();
            }
            else 
            {
                Subject = subject;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }
            var subject = await _context.Subjects.FindAsync(id);

            if (subject != null)
            {
                Subject = subject;

                try
                {
                    _context.Subjects.Remove(Subject);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                    {
                        ModelState.AddModelError(string.Empty, "Môn học đang vận hành, không thể xóa bây giờ.");
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
