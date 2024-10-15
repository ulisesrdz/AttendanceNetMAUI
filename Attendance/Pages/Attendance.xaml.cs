using Attendance.Entities;
using Attendance.VM;
using ZXing.Net.Maui;
//using ZXing.Net.Maui.Controls;
using ZXing.QrCode.Internal;
using Attendance.Helpers;
using System.Runtime.InteropServices;

namespace Attendance.Pages;

public partial class Attendance : ContentPage
{
    private bool isBusy {  get; set; }
    public int Id_Course { get; set; }
    public Attendance()
    {
		InitializeComponent();
        CreateData();
        isBusy = false;
    }

    private void CreateData()
    {
        if (BindingContext is AttendanceVM viewModel)
        {
            Entities.AttendanceEnt attendance = new Entities.AttendanceEnt();

            attendance.id_student = "1";
            attendance.date_time = DateTime.Now;
            attendance.id_course = Session.Id_Course.ToString();
            attendance.id_user = Session._IdUser.ToString();

            viewModel._LtsAttendace.Add(attendance);
            
            if (viewModel != null && viewModel.Tapped_Save_Command.CanExecute(null))
            {
                viewModel.Tapped_Save_Command.Execute(null);
            }


            attendance.id_student = "2";
            attendance.date_time = DateTime.Now;
            attendance.id_course = Session.Id_Course.ToString();
            attendance.id_user = Session._IdUser.ToString();

            viewModel._LtsAttendace.Add(attendance);

            if (viewModel != null && viewModel.Tapped_Save_Command.CanExecute(null))
            {
                viewModel.Tapped_Save_Command.Execute(null);
            }
        }
           
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        //barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!isBusy)
            {
                isBusy = true;
                if (BindingContext is AttendanceVM viewModel)
                {

                    lblTitle.Text = String.Empty;
                    Entities.AttendanceEnt attendance = new Entities.AttendanceEnt();
                    foreach (var barcode in e.Results)
                    {
                        var _barcode = barcode.Value.Split(',');
                        attendance.id_student = _barcode[0];
                        attendance.date_time = DateTime.Now;
                        attendance.id_course =  Session.Id_Course.ToString();
                        attendance.id_user=Session._IdUser.ToString();

                        lbl.Text = $"Barcodes: {barcode.Format} -> {barcode.Value}";

                        this.generateBarcode.Value = e.Results[0].Value;

                        viewModel._LtsAttendace.Add(attendance);

                        if (viewModel != null && viewModel.Tapped_Save_Command.CanExecute(null))
                        {
                            viewModel.Tapped_Save_Command.Execute(null);
                        }

                        await Application.Current.MainPage.DisplayAlert("Success", _barcode[1] + " Saved", "OK");
                        //await DisplayAlert("Success", barcode.Value + " Saved", "OK");
                        //var task = DisplayAlert("Success", barcode.Value + " Saved", "OK");

                        await Task.Delay(3000);

                        //if (task != null && !task.IsCompleted)
                        //{
                        //    await this.Navigation.PopAsync();
                        //}
                        isBusy = false;
                    }

                }
                else
                {
                    isBusy = false;
                }
            }
            
        });

        //foreach (var barcode in e.Results)
        //{
        //    lbl.Text = barcode.Value.ToString();
        ////$"Barcodes: {barcode.Format} -> {barcode.Value}";
        //}

    }
    

    async void OnButtonClick(object sender, EventArgs args)
    {
        await App.Current.MainPage.Navigation.PushModalAsync(new Pages.MainMenu());
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();        
    }

    protected override void OnDisappearing()
    {
        
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