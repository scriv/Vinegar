using System.Collections.Generic;
using System.Diagnostics;

namespace Vinegar
{
	[DebuggerDisplay("Scenario: {Title}")]
	public class Scenario : ITaggable
	{
		public Scenario()
		{
			this.Steps = new List<Step>();
		}

		public string Title { get; set; }
		public List<string> Tags { get; set; }
		public List<Step> Steps { get; set; } 
	}
}