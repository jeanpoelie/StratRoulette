namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;

	using ModelLibrary.Models;

	public enum FeedbackType
	{
		Like,

		Report
	}

	public class Challenges
	{
		public static string AddFeedback(string ip, int challengeId, FeedbackType feedbackType, string gameName)
		{
			if (DatabaseCommunication.IsIPUnique(ip, feedbackType, gameName, challengeId))
			{
				try
				{
					DatabaseCommunication.AddIPToFeedback(ip, feedbackType, gameName, challengeId);
				}
				catch (Exception ex)
				{
					return ex.Message;
				}
			}
			else
			{
				return "You have already voted.";
			}

			return "Your vote has been submitted.";
		}

		public static ChallengeModel Get(int id)
		{
			return DatabaseCommunication.GetChallenge(id).ToList<ChallengeModel>().FirstOrDefault();
		}

		public static ChallengeModel Get(string side, string difficulty, string gameName)
		{
			return DatabaseCommunication.GetChallenge(side, difficulty, gameName).ToList<ChallengeModel>().FirstOrDefault();
		}

		public static ChallengeModel GetRandom(string side, string difficulty, string peripheral, int? gameModeId, List<int> maps, string gameName)
		{
			return DatabaseCommunication.GetRandomChallenge(side, difficulty, peripheral, gameModeId, maps, gameName).ToList<ChallengeModel>().FirstOrDefault();
		}

		public static IList<ChallengeModel> GetAll(string gameName)
		{
			return DatabaseCommunication.GetAllChallenges(gameName).ToList<ChallengeModel>().OrderBy(c => c.Title).ToList();
		} 

		public static IList<MapModel> GetMaps(string challengeTitle)
		{
			return DatabaseCommunication.GetChallengeMaps(challengeTitle).ToList<MapModel>();
		}

		public static IList<GameModeModel> GetGameModes(string challengeTitle)
		{
			return DatabaseCommunication.GetChallengeGameModes(challengeTitle).ToList<GameModeModel>();
		}

		#region [Stored Procedures]


		public static ChallengeModel GetStoredProcedure(string gameName, string side, string difficulty)
		{
			var challenge = DatabaseCommunication.GetChallengeStoredProcedure(gameName, side, difficulty).ToList<ChallengeModel>().FirstOrDefault();

			DatabaseCommunication.Stats_AddChallengeStoredProcedure(challenge.Id.ToString(), challenge.Side, challenge.Difficulty);

			return challenge;
		}

		#endregion
	}
}