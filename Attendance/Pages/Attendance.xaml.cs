using Attendance.VM;
using ZXing.Net.Maui;
//using ZXing.Net.Maui.Controls;
using ZXing.QrCode.Internal;

namespace Attendance.Pages;

public partial class Attendance : ContentPage
{
	public Attendance()
    {
		InitializeComponent();
       
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        //barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (BindingContext is AttendanceVM viewModel)
            {
                lblTitle.Text = String.Empty;
                Entities.AttendanceEnt attendance = new Entities.AttendanceEnt();
                foreach (var barcode in e.Results)
                {
                    attendance.id_student = Convert.ToInt32(barcode.Value);
                    attendance.attendace_Date = DateTime.Now;
                    lbl.Text = $"Barcodes: {barcode.Format} -> {barcode.Value}";
                    
                    this.generateBarcode.Value = e.Results[0].Value;
                    
                    viewModel._LtsAttendace.Add(attendance);
                    //await Application.Current.MainPage.DisplayAlert("Success", barcode.Value + " Saved", "OK");
                    
                }

            }
        });       

    }
    async void OnButtonClick(object sender, EventArgs args)
    {
        await App.Current.MainPage.Navigation.PushModalAsync(new Pages.MainMenu());
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //this.barcodeView.CameraLocation = ZXing.Net.Maui.CameraLocation.Rear;
        //if (barcodeView == null)
        //{
        //    this.barcodeView.CameraLocation = ZXing.Net.Maui.CameraLocation.Rear;
        //    //barcodeView = global::Microsoft.Maui.Controls.NameScopeExtensions.FindByName<global::ZXing.Net.Maui.Controls.CameraBarcodeReaderView>(this, "barcodeView");
        //}
        // Este código se ejecuta cada vez que la página está a punto de mostrarse.
        // Puedes realizar acciones como cargar datos frescos aquí.
    }

    protected override void OnDisappearing()
    {
        //barcodeView = null;
    }
    private void touchOnSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        this.barcodeView.IsTorchOn = this.touchOnSwitch.IsToggled;
    }

    private void cameraSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (this.cameraSwitch.IsToggled)
        {
            this.barcodeView.CameraLocation = ZXing.Net.Maui.CameraLocation.Front;
        }
        else
        {
            this.barcodeView.CameraLocation = ZXing.Net.Maui.CameraLocation.Rear;
        }
    }

}