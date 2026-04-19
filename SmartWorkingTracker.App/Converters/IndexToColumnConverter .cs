using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SmartWorkingTracker.App.Converters
{
    public class IndexToColumnConverter
    {

        public object Convert(object value, Type targetType,
                object parameter, CultureInfo culture)
        {
            int index = (int)value;
            return index % 7;
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
