using Attendance.Helpers;
using Attendance.Pages;
using Attendance.Resources.Localization;


namespace Attendance;

public partial class MainPage : ContentPage
{
	int count = 0;    

    public MainPage()
	{
		InitializeComponent();
        DeviceDisplay.KeepScreenOn = true;
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

    protected override bool OnBackButtonPressed()
    {

        onBack();
        return base.OnBackButtonPressed();
    }

    private async void onBack()
    {
        var userChoise = await DisplayAlert(AppResource.CommonWarning, AppResource.CloseAppMsg, AppResource.CommonYes, AppResource.CommonNo);
        
        if (userChoise)
        {
            Application.Current.Quit();           
        }
        else
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
        
    }
}

