namespace Attendance.Pages;

public partial class CourseRegister : ContentPage
{
	public CourseRegister()
	{
		InitializeComponent();

        //Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

        //var backButton = new ToolbarItem
        //{
        //    Text = "Atr�s",
        //    Priority = 2,
        //    Order = ToolbarItemOrder.Default,
        //    IconImageSource = "back_button.png" // Puedes proporcionar una imagen para el bot�n si lo deseas
        //};

        //backButton.Clicked += OnBackButtonClicked;

        //ToolbarItems.Add(backButton);
    }

    //private async void OnBackButtonClicked(object sender, EventArgs e)
    //{
    //    // L�gica para manejar el evento de clic del bot�n de retroceso
    //    // Puedes, por ejemplo, navegar hacia atr�s o realizar alguna otra acci�n deseada
    //    //await Navigation.PopAsync();
    //}

}