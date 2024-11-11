using pages = Attendance.Pages;
using Microsoft.Maui.Controls;
using _business = Attendance.Helpers;

namespace Attendance;

public partial class AppShell : Shell
{
    public bool IsConditionalVisible { get; set; } = _business.BusinessURL.ReadText() == null ? true : false;
    
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;        
    }

}
