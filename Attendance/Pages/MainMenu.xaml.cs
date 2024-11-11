namespace Attendance.Pages;

public partial class MainMenu : ContentPage
{
	public MainMenu()
	{
		InitializeComponent();

        //var menuButton = new ToolbarItem
        //{
        //    Text = "Men�",
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
        // Implementa aqu� la l�gica para mostrar tu men� desplegable
        var selectedOption = await DisplayActionSheet("Selecciona una opci�n", "Cancelar", null, "Opci�n 1", "Opci�n 2", "Opci�n 3");

        if (selectedOption != null && selectedOption != "Cancelar")
        {
            // Procesa la opci�n seleccionada
            await DisplayAlert("Opci�n seleccionada", $"Has seleccionado: {selectedOption}", "OK");
        }
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {
        //await App.Current.MainPage.Navigation.PushModalAsync();
        await Navigation.PushAsync(App.Services.GetService<Attendance>());
    }
}