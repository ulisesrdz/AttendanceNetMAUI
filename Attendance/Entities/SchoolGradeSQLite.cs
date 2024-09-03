using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class SchoolGradeSQLite
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int id_user { get; set; }
        public string grade { get; set; }
        public string course_name { get; set; }
        public string groups { get; set; }
    }
}
