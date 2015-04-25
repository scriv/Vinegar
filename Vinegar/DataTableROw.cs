﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinegar
{
	public class DataTableRow
	{
		private readonly DataTable m_table;

		public DataTableRow(DataTable table)
		{
			m_table = table;
			this.Cells = new List<string>();
		}

		public string this[string header]
		{
			get 
			{
				int headerIndex = m_table.Headers.IndexOf(header);

				return this.Cells[headerIndex];
			}
		}

		public IList<string> Cells { get; set; }

		public override string ToString()
		{
			return base.ToString();
		}
	}
}