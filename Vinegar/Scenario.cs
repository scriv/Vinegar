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
		public IList<string> Tags { get; set; }
		public IList<Step> Steps { get; set; } 
	}
}