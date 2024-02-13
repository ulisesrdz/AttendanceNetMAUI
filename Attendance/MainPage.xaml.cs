using Attendance.Helpers;
using Attendance.Pages;


namespace Attendance;

public partial class MainPage : ContentPage
{
	int count = 0;    

    public MainPage()
	{
		InitializeComponent();
        //addToolItem();

    }

    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseRegister());
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseRegister());
    }
}

