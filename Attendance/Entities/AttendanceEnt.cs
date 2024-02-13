using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities
{
    public class AttendanceEnt
    {

        public int id_student { get; set; }
        public DateTime attendace_Date { get; set; }
        public int id_subject { get; set; }
        public int id_section { get; set; }
        public int id_group { get; set; }
        public AttendanceEnt() 
        {
            id_student = 0;
            attendace_Date = DateTime.MinValue;
            id_subject = 0;
            id_section = 0;
            id_group = 0;
        }


    }
}
