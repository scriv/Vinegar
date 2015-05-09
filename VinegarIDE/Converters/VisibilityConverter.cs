using System;
using System.Windows;
using System.Windows.Data;

namespace Vinegar.Ide.Converters
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType == typeof(Visibility))
			{
				return value == null ? Visibility.Collapsed : Visibility.Visible;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}