using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Tracker_App.Schemas
{
    public class CourseStatusCount
    {
        public string CourseStatus { get; set; } // e.g., "In Progress", "Completed", "Dropped", "Plan to Take"
        public int Count { get; set; }
    }
}
