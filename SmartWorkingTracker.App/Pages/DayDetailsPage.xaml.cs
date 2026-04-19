using SmartWorkingTracker.App.ViewModel;
using SmartWorkingTracker.Core.Models;

namespace SmartWorkingTracker.App.Pages;

public partial class DayDetailsPage : ContentPage
{
	public DayDetailsPage(DayDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnSessionSelected(
            object sender,
            SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is WorkSession session)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(EditSessionPage)}?sessionId={session.Id}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }

}