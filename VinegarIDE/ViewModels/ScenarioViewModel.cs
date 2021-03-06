﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Vinegar.Ide.Commands;

namespace Vinegar.Ide.ViewModels
{
	[Export]
	public class ScenarioViewModel : ViewModelBase
	{
		private Scenario m_scenario;

		public ScenarioViewModel()
		{
		}

		public Scenario Scenario
		{
			get { return m_scenario; }
			set
			{
				m_scenario = value;
				this.OnPropertyChanged(() => this.Scenario);
				this.OnPropertyChanged(() => this.Title);
			}
		}

		public string Title { get { if (this.Scenario != null) return this.Scenario.Title; else return string.Empty; } }
	}
}