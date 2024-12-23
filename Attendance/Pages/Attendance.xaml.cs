using Attendance.Entities;
using Attendance.VM;
using ZXing.Net.Maui;
//using ZXing.Net.Maui.Controls;
using ZXing.QrCode.Internal;
using Attendance.Helpers;
using System.Runtime.InteropServices;
using Attendance.Resources.Localization;
using Plugin.Maui.Audio;

namespace Attendance.Pages;

public partial class Attendance : ContentPage
{
    private bool isBusy {  get; set; }
    public int Id_Course { get; set; }

    private readonly AudioService _audioService;   

    public Attendance(AudioService audioService)
    {
        InitializeComponent();
        isBusy = false;
        _audioService = audioService;

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
                        attendance.id_student = _barcode[1];
                        attendance.date_time = DateTime.Now;
                        attendance.id_course =  Session.Id_Course.ToString();
                        attendance.id_user = Session._IdUser.ToString();
                        attendance.student_name = _barcode[0];

                        await _audioService.PlaySoundAsync("scannerbeep.mp3");

                        lbl.Text = $"Barcodes: {barcode.Format} -> {barcode.Value}";

                        this.generateBarcode.Value = e.Results[0].Value;

                        viewModel._LtsAttendace.Add(attendance);

                        if (viewModel != null && viewModel.Tapped_Save_Command.CanExecute(null))
                        {
                            viewModel.Tapped_Save_Command.Execute(null);
                        }

                        //await Application.Current.MainPage.DisplayAlert(AppResource.Common_Successful, string.Format(AppResource.Attendance_DataSaved, _barcode[1]), AppResource.Common_OK);
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