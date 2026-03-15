using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Tracker_App.Schemas
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string ItemType { get; set; } // Will display "Course" or "Assessment"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
