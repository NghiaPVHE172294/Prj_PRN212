using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectWPF.StudentManage.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            string role = value.ToString() ?? "";
            string param = parameter.ToString() ?? "";
            return role == param ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
