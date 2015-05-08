using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Vinegar.Ide.Commands;

namespace Vinegar.Ide.ViewModels
{
	[Export(typeof(EnvironmentViewModel))]
	[Export(typeof(IProgress<string>))]
	public class EnvironmentViewModel : ViewModelBase, IProgress<string>
	{
		private string m_status = string.Empty;
		private bool m_isWorking;
		private readonly ScenarioViewModel m_scenarioViewModel;
		private readonly IFeatureProvider[] m_featureProviders;
		private readonly ObservableCollection<LibrarySourceViewModel> m_library = new ObservableCollection<LibrarySourceViewModel>();

		[ImportingConstructor]
		public EnvironmentViewModel([ImportMany]IFeatureProvider[] featureProviders, ScenarioViewModel scenarioViewModel)
		{
			m_featureProviders = featureProviders;
			m_scenarioViewModel = scenarioViewModel;
			this.LoadLibrariesCommand = new AwaitableDelegateCommand(this.LoadLibraries);
		}

		public string Status
		{
			get { return m_status; }
			set
			{
				this.m_status = value;
				this.OnPropertyChanged(() => this.Status);
			}
		}

		public bool IsWorking
		{
			get { return m_isWorking; }
			set
			{
				this.m_isWorking = value;
				this.OnPropertyChanged(() => this.IsWorking);
			}
		}

		public ObservableCollection<LibrarySourceViewModel> Library
		{
			get { return m_library; }
		}

		public IAsyncCommand LoadLibrariesCommand { get; set; }

		private async Task LoadLibraries()
		{
			this.IsWorking = true;

			var librarySource = MainWindow.Container.GetExport<LibrarySourceViewModel>().Value;
			this.Library.Add(librarySource);

			librarySource.Name = m_featureProviders[0].Name;
			var features = await m_featureProviders[0].GetFeatures(this);

			foreach (Feature feature in features)
			{
				librarySource.Features.Add(feature);
			}

			this.IsWorking = false;
		}

		public void Report(string value)
		{
			this.Status = value;
		}
	}
}