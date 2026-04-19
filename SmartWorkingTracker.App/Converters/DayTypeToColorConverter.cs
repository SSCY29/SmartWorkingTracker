using SmartWorkingTracker.App.ViewModel;
using System.Globalization;

namespace SmartWorkingTracker.App.Converters
{
    public class DayTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DayType dayType)
                return Colors.Transparent;

            return dayType switch
            {
                DayType.Presence => Color.FromArgb("#D6EAF8"),      // azzurro chiaro
                DayType.SmartWorking => Color.FromArgb("#D5F5E3"),  // verde chiaro
                DayType.Mixed => Color.FromArgb("#FDEBD0"),         // arancio chiaro
                _ => Colors.Transparent
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            return null;
        }
    }
}
