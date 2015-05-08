using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
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

namespace Vinegar.Ide
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static readonly CompositionContainer Container;

		static MainWindow()
		{
			var catalog = new AggregateCatalog(
				new AssemblyCatalog(typeof(Feature).Assembly),
				new AssemblyCatalog(typeof(MainWindow).Assembly)
				);

			Container = new CompositionContainer(catalog);
		}

		public MainWindow()
		{
			InitializeComponent();
			
			this.DataContext = Container.GetExport<EnvironmentViewModel>().Value;
		}
	}
}
