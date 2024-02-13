using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.API
{
    public class LoginRequest
    {
        public string email_user { get; set; }
        public string password_user { get; set; }

        public LoginRequest(string username, string password)
        {
            this.email_user = username;
            this.password_user = password;
        }
    }
}
