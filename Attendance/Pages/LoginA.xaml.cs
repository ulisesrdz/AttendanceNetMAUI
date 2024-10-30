using Attendance.Resources.Localization;

namespace Attendance.Pages;

public partial class LoginA : ContentPage
{
	public LoginA()
	{
		InitializeComponent();

        var app = Application.Current as App;
        //if (app != null)
        //{
        //    var currentTheme = Application.Current.RequestedTheme;
        //    var newTheme = currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
        //    app.ApplyTheme(newTheme);
        //}

        //Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    private async void Tapped_For_SignUP(object sender, EventArgs e)
    {

        //await Shell.Current.GoToAsync("//Register");
    }

    private async void Tapped_For_Login(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync("//MainMenu");        
    }

    protected override bool OnBackButtonPressed()
    {

        onBack();
        return base.OnBackButtonPressed();
    }

    private async void onBack()
    {
        Application.Current.Quit();

    }
}