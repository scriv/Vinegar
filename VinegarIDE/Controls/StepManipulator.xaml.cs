using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vinegar.Ide.Controls
{
	/// <summary>
	/// Interaction logic for StepManipulator.xaml
	/// </summary>
	public partial class StepManipulator : UserControl
	{
		private Popup m_popup;

		public StepManipulator()
		{
			InitializeComponent();
		}

		//Placement
		public static readonly DependencyProperty PlacementProperty =
					Popup.PlacementProperty.AddOwner(typeof(StepManipulator));

		public PlacementMode Placement
		{
			get { return (PlacementMode)GetValue(PlacementProperty); }
			set { SetValue(PlacementProperty, value); }
		}

		//PlacementTarget
		public static readonly DependencyProperty PlacementTargetProperty =
		   Popup.PlacementTargetProperty.AddOwner(typeof(StepManipulator));

		public UIElement PlacementTarget
		{
			get { return (UIElement)GetValue(PlacementTargetProperty); }
			set { SetValue(PlacementTargetProperty, value); }
		}

		//PlacementRectangle
		public static readonly DependencyProperty PlacementRectangleProperty =
					Popup.PlacementRectangleProperty.AddOwner(typeof(StepManipulator));

		public Rect PlacementRectangle
		{
			get { return (Rect)GetValue(PlacementRectangleProperty); }
			set { SetValue(PlacementRectangleProperty, value); }
		}

		//HorizontalOffset
		public static readonly DependencyProperty HorizontalOffsetProperty =
			Popup.HorizontalOffsetProperty.AddOwner(typeof(StepManipulator));

		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}

		//VerticalOffset
		public static readonly DependencyProperty VerticalOffsetProperty =
				Popup.VerticalOffsetProperty.AddOwner(typeof(StepManipulator));

		public double VerticalOffset
		{
			get { return (double)GetValue(VerticalOffsetProperty); }
			set { SetValue(VerticalOffsetProperty, value); }
		}

		/// <summary>
		/// IsOpen
		/// </summary>
		public static readonly DependencyProperty IsOpenProperty =
			Popup.IsOpenProperty.AddOwner(
			typeof(StepManipulator),
			new FrameworkPropertyMetadata(
				false,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				new PropertyChangedCallback(IsOpenChanged)));

		public bool IsOpen
		{
			get { return (bool)GetValue(IsOpenProperty); }
			set { SetValue(IsOpenProperty, value); }
		}

		/// <summary>
		/// PropertyChangedCallback method for IsOpen Property
		/// </summary>
		/// <param name=""element""></param>
		/// <param name=""e""></param>
		private static void IsOpenChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			StepManipulator ctrl = (StepManipulator)element;

			if ((bool)e.NewValue)
			{
				if (ctrl.m_popup == null)
				{
					ctrl.HookupParentPopup();
				}
			}
		}

		/// <summary>
		/// Create the Popup and attach the CustomControl to it.
		/// </summary>
		private void HookupParentPopup()
		{
			m_popup = new Popup();

			m_popup.AllowsTransparency = true;
			m_popup.PopupAnimation = PopupAnimation.Fade;

			// Set Height and Width
			//this.Height = this.NormalHeight;
			//this.Width = this.NormalWidth;


			Popup.CreateRootPopup(m_popup, this);
		}
	}
}