using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PracticeDoosan.Converter
{
    public class BoolToStringServoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? "SERVO OFF" : "SERVO ON";
            }

            return "SERVO ON";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return text.Equals("SERVO OFF", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
