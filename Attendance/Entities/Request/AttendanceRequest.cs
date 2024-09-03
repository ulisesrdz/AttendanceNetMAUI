using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities.Request
{
    internal class AttendanceRequest
    {
        public int id_user { get; set; }
        public int id_student { get; set; }
        public int id_course {  get; set; }
        public DateTime attendaceDate { get; set; }

    }
}
