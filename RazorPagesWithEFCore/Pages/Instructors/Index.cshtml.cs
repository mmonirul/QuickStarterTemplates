﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesWithEFCore.Models;
using RazorPagesWithEFCore.Models.SchoolViewModels;

namespace RazorPagesWithEFCore.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesWithEFCore.Models.SchoolContext _context;

        public IndexModel(RazorPagesWithEFCore.Models.SchoolContext context)
        {
            _context = context;
        }

        public InstructorIndexData Instructor { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }


        public async Task OnGetAsync(int? id, int? courseID)
        {
            Instructor = new InstructorIndexData
            {
                Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                    .Include(i => i.CourseAssignments)
                        .ThenInclude(i => i.Course)
                            .ThenInclude(i => i.Enrollments)
                                .ThenInclude(i => i.Student)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync()
            };


            if (id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = Instructor.Instructors
                    .Single(i => i.ID == id.Value);

                Instructor.Courses = instructor.CourseAssignments
                    .Select(s => s.Course);
            }
            if (courseID != null)
            {
                CourseID = courseID.Value;
                Instructor.Enrollments = Instructor.Courses
                    .Single(x => x.CourseID == courseID)
                    .Enrollments;

                // Example of Explicit loading
                ////var selectedCourse = Instructor.Courses.Where(x => x.CourseID == courseID).Single();
                ////await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                ////foreach (Enrollment enrollment in selectedCourse.Enrollments)
                ////{
                ////    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                ////}
                ////Instructor.Enrollments = selectedCourse.Enrollments;

            }
        }
    }
}
