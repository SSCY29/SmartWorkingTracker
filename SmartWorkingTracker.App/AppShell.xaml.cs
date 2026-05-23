using SmartWorkingTracker.App.Pages;

namespace SmartWorkingTracker.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("sessionList", typeof(SessionListPage));
            Routing.RegisterRoute("editSession", typeof(EditSessionPage));
            Routing.RegisterRoute("editContract", typeof(EditContractPage));
        }
    }
}
