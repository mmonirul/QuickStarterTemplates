using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesWithEFCore.Models;

namespace RazorPagesWithEFCore.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyStudent = new Student();
            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate
                ))
            {
                _context.Student.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }


        // Using ViewModel 

        //[BindProperty]
        //public StudentVM StudentVM { get; set; }
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var entry = _context.Add(new Student());
        //    entry.CurrentValues.SetValues(StudentVM);
        //    await _context.SaveChangesAsync();
        //    return RedirectToPage("./Index");
        //}

    }
}