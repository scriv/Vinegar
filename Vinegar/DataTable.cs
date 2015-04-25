using System.Collections.Generic;
using System.Diagnostics;

namespace Vinegar
{
	public class DataTable
	{
		public DataTable()
		{
			this.Headers = new List<string>();
			this.Rows = new List<DataTableRow>();
		}

		public IList<string> Headers { get; set; }

		public IList<DataTableRow> Rows { get; set; }

		public override string ToString()
		{
			string gherkinSyntax = "|";
			gherkinSyntax += string.Join("|", this.Headers);
			gherkinSyntax += "|";

			return gherkinSyntax;
		}
	}
}