namespace Attendance.Pages;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();       
    }

    private async void Tapped_For_SignIn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginA");
    }
}