using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PracticeDoosan.Converter
{
    public class BoolToVisibilityConverter
    {
        // Connect 버튼 용
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // parameter 가 "invert"일 경우 반대로 처리
                if (parameter != null && parameter.ToString().Equals("invert", StringComparison.OrdinalIgnoreCase))
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;

                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        // Visibility → bool (역변환)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;

                if (parameter != null && parameter.ToString().Equals("invert", StringComparison.OrdinalIgnoreCase))
                    return !result;

                return result;
            }

            return false;
        }
    }
}
