using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971_Mobile_App_PA.Schemas
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }
        public int TermId { get; set; }

        [MaxLength(100)]
        public string CourseTitle { get; set; }
        public string CourseStatus { get; set; } // e.g., "In Progress", "Completed", "Dropped", "Plan to Take"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InstructorName { get; set; }

        [MaxLength(15)]
        public string InstructorPhone { get; set; }

        [MaxLength(50)]
        public string InstructorEmail { get; set; }
        public string notificationsEnabled { get; set; } = "true";

        [MaxLength(250)]
        public string CourseNotes { get; set; }
    }
}
