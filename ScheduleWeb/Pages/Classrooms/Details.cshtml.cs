﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScheduleCore.Entities;

namespace ScheduleWeb.Pages.Classrooms
{
    public class DetailsModel : PageModel
    {
        private readonly ScheduleCore.Entities.ScheduleContext _context;

        public DetailsModel(ScheduleCore.Entities.ScheduleContext context)
        {
            _context = context;
        }

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
    }
}