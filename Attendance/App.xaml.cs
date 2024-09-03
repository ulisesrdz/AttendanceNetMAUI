using Attendance.Handlers;
using Attendance.Pages;
using _business = Attendance.Helpers;
using Microsoft.Maui.Platform;
using Attendance.API;

namespace Attendance;

public partial class App : Application
{
    public bool IsConditionalVisible { get; set; } = _business.BusinessURL.ReadText() == null ? true : false;
    private ContentPage closingPopup;
    static SQLLiteDataBaseServices _database;

    public static SQLLiteDataBaseServices DataBase
    {
        get
        {
            if (_database == null)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Attendance.db3");
                _database = new SQLLiteDataBaseServices(dbPath);
            }
            return _database;
        }
    }

    public App()
	{
		InitializeComponent();
        InitializeClosingPopup();

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

        //ApplyTheme(AppTheme.Dark);
        //MainPage = new NavigationPage(new MainPage());
        MainPage = IsConditionalVisible ? new NavigationPage(new BusinessURL()) : new NavigationPage(new LoginA());
        //MainPage = new AppShell();
        //ApplyTheme(Application.Current.RequestedTheme);
        //Application.Current.RequestedThemeChanged += (s, a) =>
        //{
        //    ApplyTheme(a.RequestedTheme);
        //};
    }

    public void ApplyTheme(AppTheme theme)
    {
        ResourceDictionary themeResources;

        if (theme == AppTheme.Dark)
        {
            themeResources = (ResourceDictionary)Resources.MergedDictionaries.FirstOrDefault(d => d.Source.ToString().Contains("DarkTheme.xaml"));
        }
        else
        {
            themeResources = (ResourceDictionary)Resources.MergedDictionaries.FirstOrDefault(d => d.Source.ToString().Contains("LightTheme.xaml"));
        }

        // Eliminar los recursos actuales y agregar los del nuevo tema
        Resources.MergedDictionaries.Clear();
        Resources.MergedDictionaries.Add(themeResources);
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        closingPopup.IsVisible = true;
    }

    private void OnClosingHandler(object sender, EventArgs e)
    {
        // Mostrar un popup antes de cerrar la aplicación
        closingPopup.IsVisible = true;
    }

    private void InitializeClosingPopup()
    {
        closingPopup = new ContentPage
        {
            Content = new StackLayout
            {
                Children = {
                        new Label { Text = "La aplicación se está cerrando", HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand }
                    }
            }
        };

        closingPopup.IsVisible = false;
    }
}
