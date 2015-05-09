using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vinegar
{
	public class DataTable : IEnumerable<DataTableRow>
	{
		public DataTable()
		{
			this.Headers = new List<string>();
			this.Rows = new List<DataTableRow>();
		}

		public List<string> Headers { get; set; }

		public List<DataTableRow> Rows { get; set; }

		public override string ToString()
		{
			string gherkinSyntax = "|";
			gherkinSyntax += string.Join("|", this.Headers);
			gherkinSyntax += "|";

			return gherkinSyntax;
		}

		#region IEnumerable<DataTableRow> Members

		public IEnumerator<DataTableRow> GetEnumerator()
		{
			return this.Rows.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.Rows).GetEnumerator();
		}

		#endregion
	}
}