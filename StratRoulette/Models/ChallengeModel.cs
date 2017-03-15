// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChallengeModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ChallengeModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StratRoulette.Models
{
	public class ChallengeModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Difficulty { get; set; }

		public string Side { get; set; }

		public string Credits { get; set; }

		public long Likes { get; set; }

		public long Dislikes { get; set; }

		public long Reports { get; set; }

		public string GameName { get; set; }

		public int KeyboardAndMouse { get; set; }

		public int Controller { get; set; }
	}
}