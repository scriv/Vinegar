using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Vinegar.Ide.Converters
{
	public class DataTableToListConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			DataTable table = (DataTable)value;
			var items = new RowItemCollection(table);

			foreach (var row in table)
			{
				items.Add(new RowItem(row, table));
			}

			return items;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private class RowItemCollection : List<RowItem>, ITypedList
		{
			private readonly DataTable m_table;

			public RowItemCollection(DataTable table)
			{
				m_table = table;
			}

			public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
			{
				var properties = new List<RowProperty>();

				foreach (var header in m_table.Headers)
				{
					properties.Add(new RowProperty(header, m_table, null));
				}

				return new PropertyDescriptorCollection(properties.ToArray());
			}

			public string GetListName(PropertyDescriptor[] listAccessors)
			{
				return null;
			}
		}

		private class RowItem : ICustomTypeDescriptor
		{
			private readonly DataTable m_table;
			private readonly DataTableRow m_tableRow;

			public RowItem(DataTableRow row, DataTable table)
			{
				m_table = table;
				m_tableRow = row;
			}

			#region ICustomTypeDescriptor Members

			public AttributeCollection GetAttributes()
			{
				throw new NotImplementedException();
			}

			public string GetClassName()
			{
				return typeof(RowItem).Name;
			}

			public string GetComponentName()
			{
				throw new NotImplementedException();
			}

			public TypeConverter GetConverter()
			{
				throw new NotImplementedException();
			}

			public EventDescriptor GetDefaultEvent()
			{
				throw new NotImplementedException();
			}

			public PropertyDescriptor GetDefaultProperty()
			{
				throw new NotImplementedException();
			}

			public object GetEditor(Type editorBaseType)
			{
				throw new NotImplementedException();
			}

			public EventDescriptorCollection GetEvents(Attribute[] attributes)
			{
				throw new NotImplementedException();
			}

			public EventDescriptorCollection GetEvents()
			{
				throw new NotImplementedException();
			}

			public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
			{
				throw new NotImplementedException();
			}

			public PropertyDescriptorCollection GetProperties()
			{
				int cellIndex = 0;
				var properties = new List<RowProperty>();

				foreach (var cell in m_tableRow.Cells)
				{
					if (cellIndex < m_table.Headers.Count)
					{
						properties.Add(new RowProperty(m_table.Headers[cellIndex], m_table, m_tableRow));
					}

					cellIndex++;
				}

				return new PropertyDescriptorCollection(properties.ToArray());
			}

			public object GetPropertyOwner(PropertyDescriptor pd)
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		private class RowProperty : PropertyDescriptor
		{
			private readonly DataTable m_table;
			private readonly DataTableRow m_tableRow;
			private readonly string m_name;

			public RowProperty(string name, DataTable table, DataTableRow row)
				: base(name, new Attribute[0])
			{
				m_table = table;
				m_tableRow = row;
				m_name = name;
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override Type ComponentType
			{
				get { return typeof(RowItem); }
			}

			public override object GetValue(object component)
			{
				return m_tableRow[m_name];
			}

			public override bool IsReadOnly
			{
				get { return true; }
			}

			public override Type PropertyType
			{
				get { return typeof(string); }
			}

			public override void ResetValue(object component)
			{

			}

			public override void SetValue(object component, object value)
			{
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}
	}
}