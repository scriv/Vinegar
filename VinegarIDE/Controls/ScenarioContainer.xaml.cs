using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vinegar.Ide.ViewModels;

namespace Vinegar.Ide.Controls
{
	/// <summary>
	/// Interaction logic for ScenarioContainer.xaml
	/// </summary>
	public partial class ScenarioContainer : UserControl
	{
		public ScenarioContainer()
		{
			InitializeComponent();
			this.DataContext = MainWindow.Container.GetExport<ScenarioContainerViewModel>().Value;
		}
	}
}
