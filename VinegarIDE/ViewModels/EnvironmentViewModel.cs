using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Vinegar.Ide.Commands;

namespace Vinegar.Ide.ViewModels
{
	[Export]
	public class EnvironmentViewModel : IProgress<string>
	{
		private readonly IFeatureProvider[] m_featureProviders;
		private readonly ObservableCollection<LibrarySourceViewModel> m_library = new ObservableCollection<LibrarySourceViewModel>();

		[ImportingConstructor]
		public EnvironmentViewModel([ImportMany]IFeatureProvider[] featureProviders)
		{
			m_featureProviders = featureProviders;
			this.LoadLibrariesCommand = new AwaitableDelegateCommand(this.LoadLibraries);
		}

		public ObservableCollection<LibrarySourceViewModel> Library
		{
			get { return m_library; }
		}

		public IAsyncCommand LoadLibrariesCommand { get; set; }

		public async Task LoadLibraries()
		{
			var librarySource = new LibrarySourceViewModel();

			librarySource.Name = m_featureProviders[0].Name;
			var features = await m_featureProviders[0].GetFeatures(this);

			foreach (Feature feature in features)
			{
				librarySource.Features.Add(feature);
			}

			this.Library.Add(librarySource);
		}

		public void Report(string value)
		{
		}
	}
}