using System.Globalization;

namespace SmartWorkingTracker.App.Converters
{
    public class IndexToRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            int index = (int)value;
            return index / 7;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return null;
        }
    }
}