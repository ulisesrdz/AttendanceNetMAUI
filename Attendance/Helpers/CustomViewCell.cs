using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Helpers
{
    public class CustomViewCell : Microsoft.Maui.Controls.ViewCell
    {
        public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(
          nameof(SelectedBackgroundColor), typeof(Color), typeof(CustomViewCell), Colors.White);
        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
        public CustomViewCell()
        {

        }
    }
}
