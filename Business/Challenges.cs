namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Extensions;

	using Models;

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

			return "Your vote has been submitted";
		}

		public static BusinessChallengeModel Get(int id)
		{
			return DatabaseCommunication.GetChallenge(id).ToList<BusinessChallengeModel>().FirstOrDefault();
		}

		public static BusinessChallengeModel Get(string side, string difficulty, string gameName)
		{
			return DatabaseCommunication.GetChallenge(side, difficulty, gameName).ToList<BusinessChallengeModel>().FirstOrDefault();
		}

		public static BusinessChallengeModel GetRandom(string side, string difficulty, string peripheral, int? gameModeId, List<int> maps, string gameName)
		{
			return DatabaseCommunication.GetRandomChallenge(side, difficulty, peripheral, gameModeId, maps, gameName).ToList<BusinessChallengeModel>().FirstOrDefault();
		}

		public static IList<BusinessChallengeModel> GetAll(string gameName)
		{
			return DatabaseCommunication.GetAllChallenges(gameName).ToList<BusinessChallengeModel>().OrderBy(c => c.Title).ToList();
		} 

		public static IList<BusinessMapModel> GetMaps(string challengeTitle)
		{
			return DatabaseCommunication.GetChallengeMaps(challengeTitle).ToList<BusinessMapModel>();
		}

		public static IList<BusinessGameModeModel> GetGameModes(string challengeTitle)
		{
			return DatabaseCommunication.GetChallengeGameModes(challengeTitle).ToList<BusinessGameModeModel>();
		}
	}
}