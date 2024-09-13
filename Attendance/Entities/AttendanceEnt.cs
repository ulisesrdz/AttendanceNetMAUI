using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class AttendanceEnt
    {

       
        public DateTime date_time { get; set; }
        public string id_student { get; set; }
        public string id_user { get; set; }
        public string id_course { get; set; }
        public string identifier { get; set; }

        public string student_name { get; set; }
        public int counter { get; set; }
        public AttendanceEnt() 
        {
            id_student = "";
            date_time = DateTime.MinValue;
            id_user = "";
            id_course = "";
            identifier = "";
            student_name = "";

        }


    }
}
