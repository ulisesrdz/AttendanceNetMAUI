using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Entities.Request
{
    internal class CourseRequest
    {
        public int id_user { get; set; }
        public string name { get; set; }
        public string grade { get; set; }
        public string group { get; set; }

    }
}
