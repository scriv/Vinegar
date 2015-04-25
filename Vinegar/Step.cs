using System.Diagnostics;

namespace Vinegar
{
	[DebuggerDisplay("{Type} {Text}")]
	public class Step
	{
		public string Text { get; set; }
		public StepType Type { get; set; }

		public DataTable DataTable { get; set; }
	}
}