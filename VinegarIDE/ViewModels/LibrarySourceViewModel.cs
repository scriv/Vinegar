using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using Vinegar.Ide.Commands;

namespace Vinegar.Ide.ViewModels
{
	[Export(typeof(LibrarySourceViewModel))]
	public class LibrarySourceViewModel : ViewModelBase
	{
		private string m_name;
		private ObservableCollection<Feature> m_features = new ObservableCollection<Feature>();
		private readonly ICommand m_selectScenarioCommand;
		private readonly IMessenger m_messenger;

		public LibrarySourceViewModel()
		{
		}

		[ImportingConstructor]
		public LibrarySourceViewModel(IMessenger messenger)
		{
			m_messenger = messenger;

			m_selectScenarioCommand = new DelegateCommand<object>(o =>
			{
				if (o is Scenario)
				{
					messenger.Send((Scenario)o);
				}
			});
		}

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

		public ICommand SelectScenarioCommand
		{
			get { return m_selectScenarioCommand; }
		}
	}
}
