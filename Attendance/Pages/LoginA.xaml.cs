namespace Attendance.Pages;

public partial class LoginA : ContentPage
{
	public LoginA()
	{
		InitializeComponent();

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
}