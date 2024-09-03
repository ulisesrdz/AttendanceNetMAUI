using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class StudentsSQLite
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public int id_course { get; set; }
        public int id_user { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
}
