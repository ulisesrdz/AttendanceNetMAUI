using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class UserSQLite
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string identifier { get; set; }
        public string name_user { get; set; }
        public string email_user { get; set; }
        public string password { get; set; }
        public string phone_number { get; set; }
    }
}
