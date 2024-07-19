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
    public class IndexModel : PageModel
    {
        private readonly ScheduleCore.Entities.StudentManagementContext _context;

        public IndexModel(ScheduleCore.Entities.StudentManagementContext context)
        {
            _context = context;
        }

        public IList<Mark> Mark { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Marks != null)
            {
                Mark = await _context.Marks
                .Include(m => m.Classroom)
                .Include(m => m.Student)
                .Include(m => m.Subject).ToListAsync();
            }
        }
    }
}