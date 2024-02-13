using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Interface
{
    interface ILocalize
    {
        CultureInfo GetCurrentCurrentInfo();
        void SetLocale(CultureInfo ci);
        void ClearCurrentCultureInfo();
    }
}
