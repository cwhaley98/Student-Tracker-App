using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971_Mobile_App_PA.Schemas
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermId { get; set; }
        [MaxLength(100)]
        public string TermTitle { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
    }
}
