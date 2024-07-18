using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public DeleteModel(ScheduleCore.Entities.ScheduleContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return NotFound();
            }
            else 
            {
                Room = room;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FindAsync(id);

            if (room != null)
            {
                Room = room;

                try
                {
                    _context.Rooms.Remove(Room);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                    {
                        ModelState.AddModelError(string.Empty, "Phòng học đang được sử dụng, không thể xóa bây giờ.");
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
