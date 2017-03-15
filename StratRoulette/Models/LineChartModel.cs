// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineChartModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LineChartModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StratRoulette.Models
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// The line chart model.
	/// </summary>
	public class LineChartModel
	{
		/// <summary>
		/// Gets or sets the plays today.
		/// </summary>
		public int PlaysToday { get; set; }

		/// <summary>
		/// Gets or sets the plays each week.
		/// </summary>
		public List<int> PlaysEachWeek { get; set; }

		/// <summary>
		/// Gets or sets the plays each week label.
		/// </summary>
		public List<string> PlaysEachWeekLabel { get; set; }

		/// <summary>
		///  Gets or sets The text you see on the top of the line chart
		/// </summary>
		public string HeaderText { get; set; }

		/// <summary>
		///  Gets or sets The text you see on the bottom of the line chart
		/// </summary>
		public string FooterText { get; set; }

		/// <summary>
		/// The define labels.
		/// </summary>
		public void DefineLabels()
		{
			this.PlaysEachWeekLabel = new List<string>(); 
			for (var i = 1; i < this.PlaysEachWeek.Count + 1; i++)
			{
				this.PlaysEachWeekLabel.Add(i.ToString());
			}
		}

		/// <summary>
		/// The is empty.
		/// </summary>
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		public bool IsEmpty()
		{
			return (this.PlaysToday == 0 
					&& (this.PlaysEachWeek == null || !this.PlaysEachWeek.Any())
					&& (this.PlaysEachWeekLabel == null || !this.PlaysEachWeekLabel.Any()));
		}
	}
}