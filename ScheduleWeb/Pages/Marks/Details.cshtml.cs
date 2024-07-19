﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Marks
{
    public class DetailsModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public DetailsModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

      public Mark Mark { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }
            else 
            {
                Mark = mark;
            }
            return Page();
        }
    }
}
