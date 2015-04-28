using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Threading;

namespace Vinegar.Ide.ViewModels
{
	public class LibrarySourceViewModel : ViewModelBase
	{
		private string m_name;
		private ObservableCollection<Feature> m_features = new ObservableCollection<Feature>();

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
				this.OnPropertyChanged(() => Name);
			}
		}

		[Import]
		public ObservableCollection<Feature> Features
		{
			get
			{
				return m_features;
			}
			set
			{
				m_features = value;
				this.OnPropertyChanged(() => Features);
			}
		}
	}
}
