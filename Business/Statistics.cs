namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
    
	using Extensions;

	using ModelLibrary.Models;

	public class Statistics
	{
		public static void AddRandomChallenge(string difficulty, string side, string peripheral, int resultId, string gameName = "")
		{
			DatabaseCommunication.AddStatisticRandomChallenge(difficulty, side, peripheral, resultId, gameName);
		}

		public static void AddRandomOperator(bool includeRecruit)
		{
			DatabaseCommunication.AddStatisticRandomOperator(includeRecruit);
		}

		public static void AddRandomLoadout(string selectedOperator)
		{
			DatabaseCommunication.AddStatisticRandomLoadout(selectedOperator);
		}

		public static List<int> GetNumberOfWeeklyChallengesSinceStart(int id = 0, string gameName = "")
		{
			DateTime startDate = new DateTime(2016, 05, 29);
			var daysSinceStart = (DateTime.UtcNow - startDate).TotalDays;
			var numOfWeeks = Math.Ceiling(daysSinceStart / 7);
			var challengesPlayedEachWeek = new List<int>();
			for (var i = 0; i < numOfWeeks - 1; i ++)
			{
				var startGetDate = startDate.AddDays((i + 1) * 7);
				challengesPlayedEachWeek.Add(DatabaseCommunication.GetNumberOfChallengesBetweenDates(startGetDate, startGetDate.AddDays(7), id, gameName));
			}
			return challengesPlayedEachWeek;
		}

		public static int GetNumberOfChallengesSinceDate(int numberOfDaysAgo, int id = 0, string gameName = "")
		{
			return DatabaseCommunication.GetNumberOfChallengesSinceDate(numberOfDaysAgo, id, gameName);
		}

		public static IList<StatisticModel> GetTopPlayedChallenges(int top, string gameName = "")
		{
			 return DatabaseCommunication.GetTopPlayedChallenges(top, gameName).ToList<StatisticModel>(exclude: "Date");
		}

		public static IList<StatisticModel> GetTopLiked(int top, string gameName = "")
		{
			return DatabaseCommunication.GetTopLikedChallenges(top, gameName).ToList<StatisticModel>(exclude: "Date");
		}

		public static IList<StatisticModel> GetTopPlayDays(int top, string gameName = "")
		{
			return DatabaseCommunication.GetTopPlayedDays(top, gameName).ToList<StatisticModel>(exclude: "Title");
		}
	}
}