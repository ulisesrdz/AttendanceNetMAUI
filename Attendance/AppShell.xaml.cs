using pages = Attendance.Pages;
using Microsoft.Maui.Controls;
using _business = Attendance.Helpers;

namespace Attendance;

public partial class AppShell : Shell
{
    public bool IsConditionalVisible { get; set; } = _business.BusinessURL.ReadText() == null ? true : false;
    
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;

        //Routing.RegisterRoute(nameof(pages.Attendance), typeof(pages.Attendance));
        ////ShellNavigationManager.Instance.AddRoute(nameof(pages.Attendance), new ShellNavigationState(pages.Attendance, null));

        //// Agrega un ícono o un elemento de menú para abrir la página de escaneo de códigos de barras
        //var escanearItem = new ShellItem
        //{
        //    FlyoutIcon = "icon_escanear.png", // Reemplaza con tu ícono
        //    Items =
        //        {
        //            new ShellSection
        //            {
        //                Items =
        //                {
        //                    new ShellContent
        //                    {
        //                        ContentTemplate = new DataTemplate(() => new pages.Attendance())
        //                    }
        //                }
        //            }
        //        }
        //};

        //Items.Add(escanearItem);
    }

}
