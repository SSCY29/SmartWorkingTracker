using MauiAppTest.Models;
using MauiAppTest.PageModels;

namespace MauiAppTest.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}