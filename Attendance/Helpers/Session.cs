using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Helpers
{
    public static class Session
    {
        public static int status { get; set; }

        public static bool LogIn { get; set; }
        public static string _URL { get; set; }
        public static int _IdURL { get; set; }
        public static int _IdUser { get; set; }
        public static int Id_Course { get; set; }
        public static byte[] Sign { get; set; }

        public static bool schoolGrade { get; set; }
        public static bool attendanceView { get; set; }

        public static string _token { get; set; }
        public static string _name { get; set; }
        public static string _email { get; set; }
        public static string _secretKey { get; set; }

        public static string _identifier { get; set;}
        public static string phone_number { get; set;}
        

    }
}
