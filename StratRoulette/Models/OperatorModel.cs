// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperatorModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the OperatorModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StratRoulette.Models
{
	using System.Collections.Generic;

	public class OperatorModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Slogan { get; set; }

		public string Side { get; set; }

		public List<OperatorLoadoutModel> Loadout { get; set; }
	}
}