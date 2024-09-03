using Attendance.Entities;
using Attendance.Helpers;
using Attendance.VM;
using ZXing.SkiaSharp;
using SkiaSharp;
using ZXing.Common;
using ZXing.Rendering;
using ZXing;
using Attendance.Resources.Localization;

namespace Attendance.Pages;

public partial class PrintQRCodePage : ContentPage
{
    private string _savedFilePath;
    private bool _active = false;
    private bool _isbusy = false;
    private string _user = "";
    public PrintQRCodePage()
	{
		InitializeComponent();
        BindingContext = new VM.UsersVM(Session._IdUser);
        btn_compartir.IsVisible = false;
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var viewModel = BindingContext as UsersVM;
            var selectedPerson = e.SelectedItem as Students;

            viewModel.SelectedPerson = selectedPerson;
            _user = selectedPerson.name+ "," + selectedPerson.id;
            
            // Reiniciar la selección para permitir seleccionar el mismo elemento nuevamente
            //((ListView)sender).SelectedItem = null;
        }
    }

    private async void OnGenerateQRCodeClicked(object sender, EventArgs e)
    {
        if (!_isbusy)
        {
            _isbusy = true;
            
            var text = _user;
            if (!string.IsNullOrEmpty(text))
            {
                if (_active)
                {
                    btn_enter.Text = AppResource.btn_enter_tittle;
                    _active = false;
                    qrCodeImage.Source = null;
                    btn_compartir.IsVisible = false;
                    _isbusy = false;
                    return;
                }
                else
                {

                    btn_enter.Text = AppResource.CommonClean;
                    _active = true;
                    btn_compartir.IsVisible = true;
                }

                var barcodeWriter = new BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 300,
                        Height = 300
                    }
                };

                var barcodeBitmap = barcodeWriter.Write(text);

                using (var image = SKImage.FromBitmap(barcodeBitmap))
                {
                    var data = image.Encode(SKEncodedImageFormat.Png, 100);
                    qrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(data.ToArray()));

                    var fileName = Path.Combine(FileSystem.CacheDirectory, "qrcode.png");
                    File.WriteAllBytes(fileName, data.ToArray());
                    _savedFilePath = fileName;
                }

            }
            else
            {
                await DisplayAlert("Error", "Seleccione alumno para generar el código QR.", "OK");
            }
            _isbusy = false;
        }
        
       
    }

    private async void OnShareQRCodeClicked(object sender, EventArgs e)
    {
        if (!_isbusy)
        {
            _isbusy = true;
            
            if (!string.IsNullOrEmpty(_savedFilePath))
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Compartir Código QR",
                    File = new ShareFile(_savedFilePath)
                });
            }
            else
            {
                await DisplayAlert("Error", "Primero genere un código QR.", "OK");
            }

            _isbusy = false;

        }
        
    }
}

public class SkiaSharpBarcodeRenderer : IBarcodeRenderer<SKBitmap>
{
    public SKBitmap Render(BitMatrix matrix, BarcodeFormat format, string content, ZXing.Common.EncodingOptions options)
    {
        var width = matrix.Width;
        var height = matrix.Height;
        var bitmap = new SKBitmap(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bitmap.SetPixel(x, y, matrix[x, y] ? SKColors.Black : SKColors.White);
            }
        }
        return bitmap;
    }

    public SKBitmap Render(BitMatrix matrix, BarcodeFormat format, string content)
    {
        var width = matrix.Width;
        var height = matrix.Height;
        var bitmap = new SKBitmap(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bitmap.SetPixel(x, y, matrix[x, y] ? SKColors.Black : SKColors.White);
            }
        }
        return bitmap;
    }
}