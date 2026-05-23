using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SmartWorkingTracker.App.ViewModels;
using SmartWorkingTracker.Data.Database;
using SmartWorkingTracker.Data.Services;
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
                }).UseMicrocharts();                

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
            builder.Services.AddSingleton<DatabaseService>();


            // ============================
            // VIEWMODELS
            // ============================
            builder.Services.AddSingleton<DayViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<SessionListViewModel>();
            builder.Services.AddSingleton<EditSessionViewModel>();
            builder.Services.AddSingleton<ReportViewModel>();
            builder.Services.AddSingleton<ContractViewModel>();
            builder.Services.AddSingleton<EditContractViewModel>();

            var app = builder.Build();
            
            return app;
        }
    }
}
