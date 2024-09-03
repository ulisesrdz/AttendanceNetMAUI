using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class AttendanceEntSQLite
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public DateTime date_time { get; set; }
        public string id_student { get; set; }
        public string id_user { get; set; }
        public string id_course { get; set; }
    }
}
