using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using Vinegar.Ide.Commands;

namespace Vinegar.Ide.ViewModels
{
	[Export]
	public class ScenarioContainerViewModel : ViewModelBase
	{
		private int m_selectedIndex = 0;
		private readonly ObservableCollection<ScenarioViewModel> m_scenarioTabs = new ObservableCollection<ScenarioViewModel>();
		private readonly DelegateCommand<ScenarioViewModel> m_closeCommand;

		[ImportingConstructor]
		public ScenarioContainerViewModel(IMessenger messenger)
		{
			m_closeCommand = new DelegateCommand<ScenarioViewModel>(CloseScenario);
			messenger.Register<Scenario>(this, s =>
				{
					if (!m_scenarioTabs.Any(t => t.Scenario == s))
					{
						m_scenarioTabs.Add(new ScenarioViewModel { Scenario = s });
					}

					this.SelectedIndex = m_scenarioTabs.IndexOf(m_scenarioTabs.First(t => t.Scenario == s));
				});
		}

		public ObservableCollection<ScenarioViewModel> Scenarios
		{
			get { return m_scenarioTabs; }
		}

		public DelegateCommand<ScenarioViewModel> CloseScenarioCommand
		{
			get { return m_closeCommand; }
		}

		public int SelectedIndex
		{
			get { return m_selectedIndex; }
			set
			{
				m_selectedIndex = value;
				this.OnPropertyChanged(() => this.SelectedIndex);
			}
		}

		private void CloseScenario(ScenarioViewModel scenario)
		{
			this.Scenarios.Remove(scenario);
		}
	}
}
