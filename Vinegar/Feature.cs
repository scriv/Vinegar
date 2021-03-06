﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Vinegar
{
	[DebuggerDisplay("Feature: {Title}")]
	public class Feature
	{
		public Feature()
		{
			this.Scenarios = new List<Scenario>();
			this.Tags = new List<string>();
			this.Description = string.Empty;
		}

		public List<string> Tags { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Background { get; set; }
		public List<Scenario> Scenarios { get; set; }
	}
}