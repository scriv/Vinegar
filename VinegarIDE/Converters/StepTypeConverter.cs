using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Vinegar.Ide.Converters
{
	public class StepTypeConverter : IValueConverter, IMultiValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Step step = (Step)values[0];
			List<Step> steps = ((IEnumerable<Step>)values[1]).ToList();
			int stepIndex = steps.IndexOf(step);

			if (stepIndex > 0)
			{
				Step previousStep = steps[stepIndex - 1];

				if (step.Type == previousStep.Type)
				{
					return "And";
				}
			}

			return step.Type.ToString();
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}