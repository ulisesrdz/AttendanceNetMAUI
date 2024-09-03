namespace Attendance.Pages;

public partial class CourseRegister : ContentPage
{
	public CourseRegister()
	{
		InitializeComponent();

        //Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);

        //var backButton = new ToolbarItem
        //{
        //    Text = "Atrás",
        //    Priority = 2,
        //    Order = ToolbarItemOrder.Default,
        //    IconImageSource = "back_button.png" // Puedes proporcionar una imagen para el botón si lo deseas
        //};

        //backButton.Clicked += OnBackButtonClicked;

        //ToolbarItems.Add(backButton);
    }

    //private async void OnBackButtonClicked(object sender, EventArgs e)
    //{
    //    // Lógica para manejar el evento de clic del botón de retroceso
    //    // Puedes, por ejemplo, navegar hacia atrás o realizar alguna otra acción deseada
    //    //await Navigation.PopAsync();
    //}

}