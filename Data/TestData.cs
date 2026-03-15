using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Student_Tracker_App.Schemas;
using Student_Tracker_App.Services;

namespace Student_Tracker_App.Data
{
    public static class TestData
    {
        /// <summary>
        /// Seeds the database with test data for evaluator review.
        /// </summary>
        public static async Task GenerateTestDataAsync(SQLiteAsyncConnection database)
        {
            // Check if data already exists to prevent duplicating the test data
            int termCount = await database.Table<Term>().CountAsync();
            if (termCount > 0)
                return;

            // --- TERM 1: Summer 2026 (4 Courses) ---
            var term1 = new Term { TermTitle = "Summer 2026", TermStart = new DateTime(2026, 6, 1), TermEnd = new DateTime(2026, 8, 31) };
            await database.InsertAsync(term1);

            var t1c1 = new Course { TermId = term1.TermId, CourseTitle = "Intro to IT", CourseStatus = "Completed", StartDate = new DateTime(2026, 6, 1), EndDate = new DateTime(2026, 6, 20) };
            var t1c2 = new Course { TermId = term1.TermId, CourseTitle = "Web Development Foundations", CourseStatus = "Completed", StartDate = new DateTime(2026, 6, 21), EndDate = new DateTime(2026, 7, 15) };
            var t1c3 = new Course { TermId = term1.TermId, CourseTitle = "Data Management", CourseStatus = "In Progress", StartDate = new DateTime(2026, 7, 16), EndDate = new DateTime(2026, 8, 10) };
            var t1c4 = new Course { TermId = term1.TermId, CourseTitle = "Network Security", CourseStatus = "Dropped", StartDate = new DateTime(2026, 8, 11), EndDate = new DateTime(2026, 8, 31) };
            await database.InsertAllAsync(new[] { t1c1, t1c2, t1c3, t1c4 });

            await database.InsertAsync(new Assessment { CourseId = t1c1.CourseId, AssessmentTitle = "IT Fundamentals Exam", AssessmentType = "Objective", StartDate = t1c1.EndDate.AddDays(-1), EndDate = t1c1.EndDate });
            await database.InsertAsync(new Assessment { CourseId = t1c2.CourseId, AssessmentTitle = "HTML/CSS Portfolio", AssessmentType = "Performance", StartDate = t1c2.StartDate, EndDate = t1c2.EndDate });


            // --- TERM 2: Spring 2027 (6 Courses) ---
            var term2 = new Term { TermTitle = "Spring 2027", TermStart = new DateTime(2027, 1, 1), TermEnd = new DateTime(2027, 5, 31) };
            await database.InsertAsync(term2);

            var t2c1 = new Course { TermId = term2.TermId, CourseTitle = "Scripting and Programming", CourseStatus = "Completed", StartDate = new DateTime(2027, 1, 1), EndDate = new DateTime(2027, 1, 31) };
            var t2c2 = new Course { TermId = term2.TermId, CourseTitle = "Software Engineering", CourseStatus = "In Progress", StartDate = new DateTime(2027, 2, 1), EndDate = new DateTime(2027, 2, 28) };
            var t2c3 = new Course { TermId = term2.TermId, CourseTitle = "Data Structures and Algorithms", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 3, 1), EndDate = new DateTime(2027, 3, 31) };
            var t2c4 = new Course { TermId = term2.TermId, CourseTitle = "Operating Systems", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 4, 1), EndDate = new DateTime(2027, 4, 15) };
            var t2c5 = new Course { TermId = term2.TermId, CourseTitle = "Cloud Foundations", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 4, 16), EndDate = new DateTime(2027, 5, 10) };
            var t2c6 = new Course { TermId = term2.TermId, CourseTitle = "Business of IT", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 5, 11), EndDate = new DateTime(2027, 5, 31) };
            await database.InsertAllAsync(new[] { t2c1, t2c2, t2c3, t2c4, t2c5, t2c6 });

            await database.InsertAsync(new Assessment { CourseId = t2c1.CourseId, AssessmentTitle = "C++ Logic Exam", AssessmentType = "Objective", StartDate = t2c1.EndDate.AddDays(-1), EndDate = t2c1.EndDate });
            await database.InsertAsync(new Assessment { CourseId = t2c2.CourseId, AssessmentTitle = "Agile Project Plan", AssessmentType = "Performance", StartDate = t2c2.StartDate, EndDate = t2c2.EndDate });


            // --- TERM 3: Winter 2027 (5 Courses) ---
            var term3 = new Term { TermTitle = "Winter 2027", TermStart = new DateTime(2027, 11, 1), TermEnd = new DateTime(2028, 2, 28) };
            await database.InsertAsync(term3);

            var t3c1 = new Course { TermId = term3.TermId, CourseTitle = "Mobile App Development", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 11, 1), EndDate = new DateTime(2027, 11, 30) };
            var t3c2 = new Course { TermId = term3.TermId, CourseTitle = "Software Quality Assurance", CourseStatus = "Plan to Take", StartDate = new DateTime(2027, 12, 1), EndDate = new DateTime(2027, 12, 31) };
            var t3c3 = new Course { TermId = term3.TermId, CourseTitle = "Software Architecture", CourseStatus = "Plan to Take", StartDate = new DateTime(2028, 1, 1), EndDate = new DateTime(2028, 1, 31) };
            var t3c4 = new Course { TermId = term3.TermId, CourseTitle = "IT Leadership", CourseStatus = "Plan to Take", StartDate = new DateTime(2028, 2, 1), EndDate = new DateTime(2028, 2, 14) };
            var t3c5 = new Course { TermId = term3.TermId, CourseTitle = "Capstone Project", CourseStatus = "Plan to Take", StartDate = new DateTime(2028, 2, 15), EndDate = new DateTime(2028, 2, 28) };
            await database.InsertAllAsync(new[] { t3c1, t3c2, t3c3, t3c4, t3c5 });

            await database.InsertAsync(new Assessment { CourseId = t3c1.CourseId, AssessmentTitle = "Xamarin/MAUI App Build", AssessmentType = "Performance", StartDate = t3c1.StartDate, EndDate = t3c1.EndDate });
            await database.InsertAsync(new Assessment { CourseId = t3c5.CourseId, AssessmentTitle = "Final Capstone Defense", AssessmentType = "Performance", StartDate = t3c5.EndDate.AddDays(-1), EndDate = t3c5.EndDate });
        }
    }
}
