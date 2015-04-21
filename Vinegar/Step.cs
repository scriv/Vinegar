using System.Diagnostics;

namespace Vinegar
{
	[DebuggerDisplay("{Text}")]
	public class Step
	{
		public string Text { get; set; }
		public StepType Type { get; set; }
	}
}