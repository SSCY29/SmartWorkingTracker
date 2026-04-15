using CommunityToolkit.Mvvm.Input;
using MauiAppTest.Models;

namespace MauiAppTest.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}