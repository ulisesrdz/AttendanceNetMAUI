using Attendance.Handlers;
using Attendance.Pages;
using _business = Attendance.Helpers;
using Microsoft.Maui.Platform;

namespace Attendance;

public partial class App : Application
{
    public bool IsConditionalVisible { get; set; } = _business.BusinessURL.ReadText() == null ? true : false;
    public App()
	{
		InitializeComponent();
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });



        MainPage = IsConditionalVisible ? new NavigationPage(new BusinessURL()) : new NavigationPage(new LoginA());
        //MainPage = new AppShell();
        
    
    }
}
