namespace Attendance.Pages;

public partial class MainMenu : ContentPage
{
	public MainMenu()
	{
		InitializeComponent();

        //var menuButton = new ToolbarItem
        //{
        //    Text = "Menú",
        //    IconImageSource = "menu_icon.png",
        //    Priority = 0,
        //    Order = ToolbarItemOrder.Primary,
        //    Command = new Command(ShowMenu)
        //};

        //ToolbarItems.Add(menuButton);

        NavigationPage.SetHasBackButton(this, false);
    }

    private async void ShowMenu()
    {
        // Implementa aquí la lógica para mostrar tu menú desplegable
        var selectedOption = await DisplayActionSheet("Selecciona una opción", "Cancelar", null, "Opción 1", "Opción 2", "Opción 3");

        if (selectedOption != null && selectedOption != "Cancelar")
        {
            // Procesa la opción seleccionada
            await DisplayAlert("Opción seleccionada", $"Has seleccionado: {selectedOption}", "OK");
        }
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {
        //await App.Current.MainPage.Navigation.PushModalAsync();
        await Navigation.PushAsync(App.Services.GetService<Attendance>());
    }
}