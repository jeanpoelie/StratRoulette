
namespace StratRoulette.Extensions
{
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	using Extensions;

	/// <summary>
	/// Contains extension methods for the <see cref="T:System.Data.DataTable"/> class.
	/// </summary>
	public static class DataTableExtensions
	{
		/// <summary>
		/// Returns a <see cref="T:System.Collections.Generic.IList`1"/> with objects populated from the <see cref="T:System.Data.DataTable"/>.
		/// </summary>
		/// <typeparam name="T">The underlying <see cref="T:System.Type"/> of the list to populate.</typeparam>
		/// <param name="table">The <see cref="T:System.Data.DataTable"/> this method extends.</param>
		/// <param name="include">A comma-separated list of Properties to include in the mapping.</param>
		/// <param name="exclude">A comma-separated list of Properties to exclude from the mapping.</param>
		/// <returns>A <see cref="T:System.Collections.Generic.IList`1"/> with objects populated from the <see cref="T:System.Data.DataTable"/>.</returns>
		public static IList<T> ToList<T>(this DataTable table, string include = "", string exclude = "") where T : new()
		{
			var properties = DataRowExtensionMethods.GetSelectedProperties<T>(include, exclude);
			return table.AsEnumerable().Select(row => DataRowExtensionMethods.CreateItemFromRow<T>(row, properties)).ToList();
		}
	}
}
