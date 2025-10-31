using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace PracticeDoosan.Converter
{
    public class BoolToColorServoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOn)
            {
                return isOn ?
                    new SolidColorBrush(Color.FromRgb(239, 68, 68)) :   // 빨간색 (OFF)
                new SolidColorBrush(Color.FromRgb(34, 197, 94));  // 초록색 (ON)
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
