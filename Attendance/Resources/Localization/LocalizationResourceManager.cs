using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Resources.Localization
{
    class LocalizationResourceManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static LocalizationResourceManager Instance { get; } = new();

        public object this[string resourceKey] => AppResource.ResourceManager.GetObject(resourceKey, AppResource.Culture) ?? Array.Empty<byte>();

        private LocalizationResourceManager()
        {
            AppResource.Culture = CultureInfo.CurrentCulture;
        }

        public void SetCulture(CultureInfo culture)
        {
            AppResource.Culture = culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
