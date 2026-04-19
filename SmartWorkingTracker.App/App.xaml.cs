using SmartWorkingTracker.App.Pages;

namespace SmartWorkingTracker.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DayDetailsPage), typeof(DayDetailsPage));
            Routing.RegisterRoute(nameof(EditSessionPage), typeof(EditSessionPage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}