using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971_Mobile_App_PA.Schemas
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }
        public int CourseId { get; set; }

        public string AssessmentTitle { get; set; }
        public string AssessmentType { get; set; } // e.g., "Objective" or "Performance"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool notificationsEnabled { get; set; } = true;

    }
}
