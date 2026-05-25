using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartWorkingTracker.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
                
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {

                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(); // 🔥 NON mettere il nome
                }

            }
        }
    }
}
