using Attendance.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Platforms.Android
{
    class ILocalize_Android : ILocalize

    {
        private CultureInfo _ci;
        public void ClearCurrentCultureInfo()
        {
            _ci = null;
        }

        public CultureInfo GetCurrentCurrentInfo()
        {
            if (_ci != null)
                return _ci;

            var netLanguage = "en";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = androidLocale.ToString().Replace("_", "-");

            try
            {
                _ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException)
            {
                _ci = new System.Globalization.CultureInfo("en");
            }

            return _ci;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
