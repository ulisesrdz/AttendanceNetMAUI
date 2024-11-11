using Attendance.Helpers;
using CommunityToolkit.Maui;
using pages = Attendance.Pages;
#if ANDROID
using Attendance.Platforms.Android;
#endif
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;


namespace Attendance;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{

       

    var builder = MauiApp.CreateBuilder();
		
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader().UseMauiCommunityToolkitMediaElement()       
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
             .ConfigureMauiHandlers(h =>
             {
                 h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                 h.AddHandler(typeof(ZXing.Net.Maui.Controls.CameraView), typeof(CameraViewHandler));
                 h.AddHandler(typeof(ZXing.Net.Maui.Controls.BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
#if ANDROID
                 h.AddHandler<CustomViewCell, CustomViewCellHandler>();
#endif
             });

        builder.Services.AddSingleton<IAudioManager>(AudioManager.Current);
        builder.Services.AddSingleton<AudioService>();
        builder.Services.AddTransient<pages.Attendance>();
        //builder.Services.AddSingleton<Attendance>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
    
}
