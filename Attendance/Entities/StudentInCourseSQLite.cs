using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class StudentInCourseSQLite
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }        
        public string id_student { get; set; }       
        public string id_course { get; set; }
    }
}
