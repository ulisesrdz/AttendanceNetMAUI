using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    class Users
    {
        public int id { get; set; }
        public string user_name { get; set; }

        public string Token_user { get; set; }
        public string email_user { get; set; }

        public string client_secret { get; set; }
    }
}
