using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student_Tracker_App.Schemas;
using Student_Tracker_App.Services;

namespace Student_Tracker_App.Data
{
    public class TestData
    {
        public async Task PopulateTestDataAsync()
        {
            try
            {
                // 1. Check if data already exists to avoid duplicates
                var existingTerms = await App.Database.GetTermsAsync();
                if (existingTerms.Count > 0)
                {
                    return; // Data already exists, do nothing
                }

                // 2. Add a Sample Term
                var term = new Term
                {
                    TermTitle = "Spring 2026",
                    TermStart = DateTime.Today,
                    TermEnd = DateTime.Today.AddMonths(6)
                };
                await App.Database.SaveTermAsync(term);

                // 3. Add a Sample Course
                var course = new Course
                {
                    TermId = term.TermId, // Link to the term we just created
                    CourseTitle = "Mobile App Development",
                    CourseStatus = "In Progress",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(1),
                    InstructorName = "Anika Patel",
                    InstructorPhone = "555-123-4567",
                    InstructorEmail = "anika.patel@strimeuniversity.edu",
                    notificationsEnabled = "true", // Note: Your Course model uses string "true"/"false"
                    CourseNotes = "This course covers .NET MAUI development."
                };
                await App.Database.SaveCourseAsync(course);

                // 4. Add Assessments
                // Assessment 1: Performance
                var assessment1 = new Assessment
                {
                    CourseId = course.CourseId, // Link to the course
                    AssessmentTitle = "Build a UI Prototype",
                    AssessmentType = "Performance",
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(10),
                    notificationsEnabled = true // Note: Your Assessment model uses bool
                };
                await App.Database.SaveAssessmentAsync(assessment1);

                // Assessment 2: Objective
                var assessment2 = new Assessment
                {
                    CourseId = course.CourseId,
                    AssessmentTitle = "C# Fundamentals Exam",
                    AssessmentType = "Objective",
                    StartDate = DateTime.Today.AddDays(20),
                    EndDate = DateTime.Today.AddDays(20),
                    notificationsEnabled = true
                };
                await App.Database.SaveAssessmentAsync(assessment2);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error populating data: {ex.Message}");
            }
        }
    }
}
