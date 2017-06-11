using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Business.Extensions
{
	using System;
	using System.Data;

	/// <summary>
	/// Contains extension methods for the <see cref="T:System.Data.DataRow"/> class.
	/// </summary>
	public static class DataRowExtensionMethods
	{
		/// <summary>
		/// Gets the value of a cell with ability of a defaultValue if the cell is <see cref="T:System.DBNull"/>.
		/// </summary>
		/// <typeparam name="T">The type of the data stored in the <see cref="T:System.Data.DataRow"/> item.</typeparam>
		/// <param name="record">The record</param>
		/// <param name="columnName">The name of the column</param>
		/// <param name="defaultValue">The value to return if the column contains <see cref="F:System.DBNull.Value"/>.</param>
		/// <returns>The value of the selected column</returns>
		public static T Field<T>(this DataRow record, string columnName, T defaultValue)
		{
			var value = record[columnName];

			if (Convert.IsDBNull(value) || value == null)
			{
				return defaultValue;
			}

			return (T)value;
		}

		/// <summary>
		/// Gets the value of a cell with ability of a defaultValue if the cell is <see cref="T:System.DBNull"/>.
		/// </summary>
		/// <typeparam name="T">The type of the data stored in the <see cref="T:System.Data.DataRow"/> item.</typeparam>
		/// <param name="record">The record</param>
		/// <param name="columnIndex">The index of the column</param>
		/// <param name="defaultValue">The value to return if the column contains <see cref="F:System.DBNull.Value"/>.</param>
		/// <returns>The value of the selected column</returns>
		public static T Field<T>(this DataRow record, int columnIndex, T defaultValue)
		{
			var value = record[columnIndex];

			if (Convert.IsDBNull(value) || value == null)
			{
				return defaultValue;
			}

			return (T)value;
		}

		public static IList<T> ToList<T>(this DataRow[] table) where T : new()
		{
			IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();

			return table.Select(row => CreateItemFromRow<T>(row, properties)).ToList();
		}

		/// <summary>
		/// Maps the <see cref="T:System.Data.DataColumn"/>s in the <see cref="T:System.Data.DataRow"/> to matching properties in the desired object.
		/// </summary>
		/// <typeparam name="T">The <see cref="T:System.Type"/> of the object to populate.</typeparam>
		/// <param name="row">The <see cref="T:System.Data.DataRow"/> this method extends.</param>
		/// <param name="include">A comma-separated list of Properties to include in the mapping.</param>
		/// <param name="exclude">A comma-separated list of Properties to exclude from the mapping.</param>
		/// <returns>An object of type <typeparamref name="T"/> with it's properties populated with data from the <see cref="T:System.Data.DataRow"/>.</returns>
		public static T ToObject<T>(this DataRow row, string include = "", string exclude = "") where T : new()
		{
			var properties = GetSelectedProperties<T>(include, exclude);

			return CreateItemFromRow<T>(row, properties);
		}

		internal static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
		{
			var item = new T();

			foreach (var property in properties)
			{
				if (!property.CanWrite)
				{
					continue;
				}

				if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType == typeof(string))
				{
					var propertyType = property.PropertyType;

					// special case for char...
					if (propertyType == typeof(char))
					{
						if (row.Table.Columns[property.Name].DataType != typeof(string))
						{
							throw new InvalidCastException(string.Format("Cannot cast from {0} to {1}", row.Table.Columns[property.Name].DataType, typeof(char)));
						}

						property.SetValue(item, (row[property.Name] == DBNull.Value) ? (object)null : row[property.Name].ToString()[0], null);
						continue;
					}

					// ...and nullable types...
					if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						var typeCol = propertyType.GetGenericArguments();

						if (typeCol.Length > 0)
						{
							var nullableType = typeCol[0];
							object o;

							if (nullableType.BaseType == typeof(Enum))
							{
								o = Enum.Parse(nullableType, row[property.Name].ToString());
							}
							else
							{
								o = (row[property.Name] == DBNull.Value) ? null : row[property.Name];
							}

							property.SetValue(item, o, null);
							continue;
						}
					}

					property.SetValue(item, (row[property.Name] == DBNull.Value) ? null : row[property.Name], null);
				}
			}

			return item;
		}

		internal static IList<PropertyInfo> GetSelectedProperties<T>(string include, string exclude)
		{
			var propertyInfos = new List<PropertyInfo>();
			var props = typeof(T).GetProperties();

			// Do included properties first
			if (!string.IsNullOrEmpty(include))
			{
				var includeProps = include.Split(',').Select(s => s.Trim()).ToList();

				foreach (var item in props)
				{
					var propName = includeProps.FirstOrDefault(a => StringComparer.OrdinalIgnoreCase.Equals(a, item.Name));

					if (!string.IsNullOrEmpty(propName))
					{
						propertyInfos.Add(item);
					}
				}
			}
			else if (!string.IsNullOrEmpty(exclude))
			{
				var excludeProps = exclude.Split(',').Select(s => s.Trim()).ToList();

				foreach (var item in props)
				{
					var propName = excludeProps.FirstOrDefault(a => StringComparer.OrdinalIgnoreCase.Equals(a, item.Name));

					if (string.IsNullOrEmpty(propName))
					{
						propertyInfos.Add(item);
					}
				}
			}
			else
			{
				propertyInfos.AddRange(props);
			}

			return propertyInfos;
		}
	}
}
