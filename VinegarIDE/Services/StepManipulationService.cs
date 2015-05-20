using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using Vinegar.Ide.Controls;

namespace Vinegar.Ide.Services
{
	/// <summary>
	/// Service class for manipulating steps within a scenario.
	/// </summary>
	public static class StepManipulationService
	{
		private static StepManipulator m_manipulator;
		private static FrameworkElement m_currentElement;

		#region Attached Properties

		/// <summary>
		///     The DependencyProperty for the Step property.
		/// </summary>
		public static readonly DependencyProperty StepProperty =
				DependencyProperty.RegisterAttached(
						"Step",              // Name
						typeof(Step),         // Type
						typeof(StepManipulationService), // Owner
						new FrameworkPropertyMetadata((object)null, PropertyChanged));

		/// <summary>
		///     Gets the value of the Step property on the specified object.
		/// </summary>
		/// <param name="element">The object on which to query the Step property.</param>
		/// <returns>The value of the Step property.</returns>
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static object GetStep(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			return element.GetValue(StepProperty);
		}

		/// <summary>
		///     Sets the Step property on the specified object.
		/// </summary>
		/// <param name="element">The object on which to set the Step property.</param>
		/// <param name="value">The value of the Step property.</param>
		public static void SetStep(DependencyObject element, object value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			element.SetValue(StepProperty, value);
		}

		/// <summary>
		///     The DependencyProperty for the Scenario property.
		/// </summary>
		public static readonly DependencyProperty ScenarioProperty =
				DependencyProperty.RegisterAttached(
						"Scenario",              // Name
						typeof(Scenario),         // Type
						typeof(StepManipulationService), // Owner
						new FrameworkPropertyMetadata((object)null, PropertyChanged));

		/// <summary>
		///     Gets the value of the Scenario property on the specified object.
		/// </summary>
		/// <param name="element">The object on which to query the Scenario property.</param>
		/// <returns>The value of the Scenario property.</returns>
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static object GetScenario(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			return element.GetValue(ScenarioProperty);
		}

		/// <summary>
		///     Sets the Scenario property on the specified object.
		/// </summary>
		/// <param name="element">The object on which to set the Scenario property.</param>
		/// <param name="value">The value of the Scenario property.</param>
		public static void SetScenario(DependencyObject element, object value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			element.SetValue(ScenarioProperty, value);
		}

		#endregion

		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			return;
			FrameworkElement element = d as FrameworkElement;

			if (element != null)
			{
				element.MouseEnter += (s, me) =>
					{
						if (m_currentElement == element)
						{
							return;
						}

						if (m_manipulator == null)
						{
							var manipulator = new StepManipulator();
							manipulator.Placement = PlacementMode.Right;
							m_manipulator = manipulator;
						}

						m_manipulator.IsOpen = false;
						m_manipulator.PlacementTarget = element;
						m_manipulator.VerticalOffset = element.ActualHeight - 10;
						m_manipulator.HorizontalOffset = m_manipulator.ActualWidth - 30;
						m_manipulator.IsOpen = true;

						m_currentElement = element;
					};

				element.MouseLeave += (s, me) =>
					{

					};
			}
		}
	}
}