using Microsoft.Extensions.Logging;
using SmartWorkingTracker.App.ViewModel;
using SmartWorkingTracker.Core.Services;
using SmartWorkingTracker.Data.Database;
using SQLitePCL;

namespace SmartWorkingTracker.App
{
    public static class MauiProgram
    {        
        public static MauiApp CreateMauiApp()
        {
            Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<AppDatabase>(sp =>
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, "smartworking.db");

                return new AppDatabase(path);
                
            });

            // ============================
            // SERVICES
            // ============================
            builder.Services.AddSingleton<WorkContractsService>();
            builder.Services.AddSingleton<WorkSessionsService>();

            // ============================
            // VIEWMODELS
            // ============================
            builder.Services.AddSingleton<MainViewModel>();


            var app = builder.Build();
            
            return app;
        }
    }
}
