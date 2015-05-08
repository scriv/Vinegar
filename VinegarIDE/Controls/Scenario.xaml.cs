using System.Windows.Controls;
using Vinegar.Ide.ViewModels;

namespace Vinegar.Ide.Controls
{
	/// <summary>
	/// Interaction logic for Scenario.xaml
	/// </summary>
	public partial class Scenario : UserControl
	{
		public Scenario()
		{
			InitializeComponent();

			this.DataContext = MainWindow.Container.GetExport<ScenarioViewModel>().Value;
		}
	}
}
