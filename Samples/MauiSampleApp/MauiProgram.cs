using MauiSampleApp.Views;
using Microsoft.Extensions.Logging;
using ObservableViewSample.Service;
using ObservableViewSample.ViewModel;

namespace MauiSampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddLogging(b =>
            {
                b.ClearProviders();
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddDebug();
            });

            builder.Services.AddSingleton<IMallManager, MallManager>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            return builder.Build();
        }
    }
}
