namespace Business
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;

	using MySql.Data.MySqlClient;

	public class DatabaseCommunication
	{
		private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

		private static readonly Database Database = new Database(ConnectionString);

		public static void AddLog(Exception ex)
		{
			const string Query = @"INSERT INTO `stratroulette`.`logging` (`Message`, `Source`, `Stacktrace`, `CreateDate`) VALUES (@Message, @Source, @Stacktrace, UTC_DATE())";

			Database.ExecuteQuery(Query, new MySqlParameter("@Message", ex.Message), new MySqlParameter("@Source", ex.Source), new MySqlParameter("@Stacktrace", ex.StackTrace));
		}

		public static void AddStatisticRandomChallenge(string difficulty, string side, string peripheral, int resultId, string gameName)
		{
			const string Query = @"INSERT INTO `statistics`.`stratroulette_randomchallenge` (`DifficultyChosen`, `SideChosen`, `ChallengeId`, `CreateDate`) VALUES (@Difficulty, @Side, @ChallengeId, UTC_DATE())";

			Database.ExecuteQuery(Query, new MySqlParameter("@Difficulty", difficulty), new MySqlParameter("@Side", side ?? "Both"), new MySqlParameter("@ChallengeId", resultId));
		}

		public static void AddStatisticRandomOperator(bool includeRecruits)
		{
			const string Query = @"INSERT INTO `statistics`.`stratroulette_randomoperator` (`IncludeRecruits`, `CreateDate`) VALUES (@IncludeRecruits, UTC_DATE())";

			Database.ExecuteQuery(Query, new MySqlParameter("@IncludeRecruits", includeRecruits));
		}

		public static void AddStatisticRandomLoadout(string selectedOperator)
		{
			const string Query = @"INSERT INTO `statistics`.`stratroulette_randomloadout` (`SelectedOperator`, `CreateDate`) VALUES (@selectedOperator, UTC_DATE())";

			Database.ExecuteQuery(Query, new MySqlParameter("@SelectedOperator", selectedOperator));
		}

		public static void AddIPToFeedback(string ip, FeedbackType feedbackkind, string gameName, int challengeId)
		{
			const string Query = @"INSERT INTO `stratroulette`.`feedback` (`IP`, `GameName`, `FeedbackKind`, `ChallengeId`) VALUES (@IP, @GameName, @FeedbackKind, @ChallengeId)";

			Database.ExecuteQuery(Query, new MySqlParameter("@IP", ip), new MySqlParameter("@GameName", gameName), new MySqlParameter("@FeedbackKind", feedbackkind.ToString()), new MySqlParameter("@ChallengeId", challengeId));
		}


		public static DataTable GetAllChallenges(string gameName)
		{
			const string Query = @"SELECT *,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Like' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Likes,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Report' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Reports,
0 AS Dislikes
FROM `stratroulette`.`challenges` WHERE `GameName` = @GameName AND `Disabled` = 0";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@GameName", gameName)).Tables[0];

			return dt;
		}

		public static DataTable GetAllVideos()
		{
			const string Query = @"SELECT * FROM `stratroulette`.`promotionalvideos` ORDER BY `Date` DESC";

			var dt = Database.ExecuteQuery(Query).Tables[0];

			return dt;
		}

		public static DataTable GetAllMaps(string gameName)
		{
			const string Query = @"SELECT Id, Name FROM `stratroulette`.`maps` WHERE GameName=@GameName";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@GameName", gameName)).Tables[0];

			return dt;
		}

		public static DataTable GetAllGameModes(string gameName)
		{
			const string Query = @"SELECT Id, Name FROM `stratroulette`.`gamemodes` WHERE GameName=@GameName";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@GameName", gameName)).Tables[0];

			return dt;
		}

		public static DataTable GetAllOperators()
		{
			const string Query = @"SELECT * FROM `stratroulette`.`operators`";

			var dt = Database.ExecuteQuery(Query).Tables[0];

			return dt;
		}

		public static DataTable GetOperatorsOnSide(string side, int number, bool includeRecruit)
		{
			const string Query = @"SELECT * FROM `stratroulette`.`operators` WHERE SpecialOps != @ExcludedSpecialOps AND (`Side`=@Side OR `Side`='Both') ORDER BY RAND()";
			
			var excludedSpecialOps = "NULL";
			if (!includeRecruit)
			{
				excludedSpecialOps = "Recruit";
			}

			var queryPart = string.Empty;
			if (number > 0)
			{
				queryPart = " LIMIT @Number";
			}

			var dt = Database.ExecuteQuery(Query + queryPart, new MySqlParameter("@Side", side), new MySqlParameter("@Number", number), new MySqlParameter("@ExcludedSpecialOps", excludedSpecialOps)).Tables[0];

			return dt;
		}

		public static DataTable GetChallenge(int id)
		{
			const string Query = @"SELECT *,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Like' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Likes,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Report' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Reports,
0 AS Dislikes
FROM `stratroulette`.`challenges` WHERE `Id`=@id";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Id", id)).Tables[0];

			return dt;
		}

		public static DataTable GetChallenge(string side, string difficulty, string gameName)
		{
			const string Query = @"SELECT *,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Like' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Likes,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Report' AND `ChallengeId` = `stratroulette`.`challenges`.`Id`) AS Reports,
0 AS Dislikes
FROM `stratroulette`.`challenges` WHERE `GameName` = @GameName AND `Disabled` = 0";
			var wherePart = string.Empty;

			if (!string.IsNullOrEmpty(side))
			{
				wherePart += " AND `Side`= @Side";
			}

			if (!string.IsNullOrEmpty(difficulty))
			{
				wherePart += " AND `Difficulty`= @Difficulty";
			}

			var dt = Database.ExecuteQuery(Query + wherePart, new MySqlParameter("@GameName", gameName), new MySqlParameter("@Side", side), new MySqlParameter("@Difficulty", difficulty)).Tables[0];

			return dt;
		}

		public static DataTable GetChallengeGameModes(string challengeTitle)
		{
			const string Query = @"SELECT DISTINCT `sg`.`Name`, `sg`.`Id` FROM `stratroulette`.`challenge_gamemodes`
INNER JOIN `stratroulette`.`gamemodes` `sg` ON `sg`.`Id` = `GameModeId`
INNER JOIN `stratroulette`.`challenges` `sc` ON `sc`.`Id` = `ChallengeId` WHERE `Title` = @ChallengeTitle";
			
			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@ChallengeTitle", challengeTitle)).Tables[0];

			return dt;
		}

		public static DataTable GetChallengeMaps(string challengeTitle)
		{
			const string Query = @"SELECT DISTINCT `sm`.`Name`, `sm`.`Id` FROM `stratroulette`.`challenge_maps`
INNER JOIN `stratroulette`.`maps` `sm` ON `sm`.`Id` = `MapId`
INNER JOIN `stratroulette`.`challenges` `sc` ON `sc`.`Id` = `ChallengeId` WHERE `Title` = @ChallengeTitle";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@ChallengeTitle", challengeTitle)).Tables[0];

			return dt;
		}

		public static bool IsIPUnique(string ip, FeedbackType feedbackkind, string gameName, int challengeId)
		{
			const string Query = @"SELECT * FROM `stratroulette`.`feedback` WHERE `IP` = @IP AND `GameName` = @GameName AND `FeedbackKind` = @FeedbackKind AND `ChallengeId`=@ChallengeId";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@IP", ip), new MySqlParameter("@GameName", gameName), new MySqlParameter("@FeedbackKind", feedbackkind.ToString()), new MySqlParameter("@ChallengeId", challengeId)).Tables[0];

			return dt.Rows.Count == 0;
		}

		public static DataTable GetOperatorLoadout(string operatorName)
		{
			const string Query =
				@"SELECT `LoadoutName` AS `Name`, `Type` FROM ( SELECT `OperatorName`, `LoadoutName`, `Type` FROM `stratroulette`.`operator_loadout` INNER JOIN `stratroulette`.`loadout` ON `Name` = `LoadoutName` ORDER BY RAND() LIMIT 5000) AS OL WHERE `OperatorName` = @OperatorName GROUP BY(Type)";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@OperatorName", operatorName)).Tables[0];

			return dt;
		}

		public static DataTable GetRandomChallenge(string side, string difficulty, string peripheral, int? gameModeId, List<int> mapids, string gameName)
		{
			const string Query = @"SELECT *,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Like' AND `ChallengeId` = `sc`.`Id`) AS Likes,
(SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Report' AND `ChallengeId` = `sc`.`Id`) AS Reports,
0 AS Dislikes
FROM `stratroulette`.`challenges` `sc`
INNER JOIN `stratroulette`.`challenge_gamemodes` `cmd` ON `cmd`.`ChallengeId` = `sc`.`Id` AND `sc`.`GameName` = @GameName
INNER JOIN `stratroulette`.`challenge_maps` `cm` ON `cm`.`ChallengeId` = `sc`.`Id` AND `sc`.`GameName` = @GameName";

			var wherePart = " WHERE `sc`.`GameName` = @GameName AND `Disabled` = 0";

			if (!string.IsNullOrEmpty(side))
			{
				wherePart += " AND (`Side`=@Side OR `Side`='Both')";
			}

			if (!string.IsNullOrEmpty(difficulty) && difficulty != "All")
			{
				wherePart += " AND `Difficulty`=@Difficulty";
			}

			if (!string.IsNullOrEmpty(peripheral) && peripheral != "Both")
			{
				switch (peripheral)
				{
					case "KeyboardMouse":
						wherePart += " AND `KeyboardAndMouse` = 1";
						break;
					case "Controller":
						wherePart += " AND `Controller` = 1";
						break;
				}
			}
			else
			{
				wherePart += " AND `KeyboardAndMouse` = 1 OR `Controller` = 1";
			}

			if (gameModeId != 0 && gameModeId != null)
			{
				wherePart += " AND `GameModeId` = @GameModeId";
			}

			if (mapids.Any())
			{
				wherePart += " AND `MapId` IN (" + string.Join(", ", mapids) + ")";
			}

			var dt = Database.ExecuteQuery(Query + wherePart + " ORDER BY RAND() LIMIT 1", new MySqlParameter("@GameName", gameName), new MySqlParameter("@Side", side), new MySqlParameter("@Difficulty", difficulty), new MySqlParameter("@GameModeId", gameModeId)).Tables[0];

			return dt;
		}

		public static int GetNumberOfChallengesBetweenDates(DateTime startDate, DateTime endDate, int id = 0, string gameName = "")
		{
			const string Query =
				@"SELECT COUNT(*) 
FROM `statistics`.`stratroulette_randomchallenge` `sc`
INNER JOIN `stratroulette`.`challenges` `cmd` ON `cmd`.`Id` = `sc`.`ChallengeId`
WHERE `CreateDate` >= @StartDate 
AND `CreateDate` < @EndDate";

			var extraFilter = string.Empty;
			if (id != 0)
			{
				extraFilter += " AND `ChallengeId` = @Id";
			}

			if (!string.IsNullOrEmpty(gameName))
			{
				extraFilter += " AND `GameName` = @GameName";
			}

			var dt =
				Database.ExecuteQuery(
					Query + extraFilter,
					new MySqlParameter("@StartDate", startDate),
					new MySqlParameter("@EndDate", endDate),
					new MySqlParameter("@Id", id),
					new MySqlParameter("@GameName", gameName)).Tables[0];

			return dt != null && dt.Rows.Count > 0 ? int.Parse(dt.Rows[0].ItemArray[0].ToString()) : 0;
		}

		public static int GetNumberOfChallengesSinceDate(int numberOfDaysAgo, int id = 0, string gameName = "")
		{
			const string Query =
				@"SELECT COUNT(*) 
FROM `statistics`.`stratroulette_randomchallenge` `sc`
INNER JOIN `stratroulette`.`challenges` `cmd` ON `cmd`.`Id` = `sc`.`ChallengeId` 
WHERE `CreateDate` >= @CreateDate";

			var extraFilter = string.Empty;
			if (id != 0)
			{
				extraFilter += " AND `ChallengeId` = @Id";
			}

			if (!string.IsNullOrEmpty(gameName))
			{
				extraFilter += " AND `GameName` = @GameName";
			}

			var dt =
				Database.ExecuteQuery(
					Query + extraFilter,
					new MySqlParameter("@CreateDate", DateTime.UtcNow.AddDays(-numberOfDaysAgo)),
					new MySqlParameter("@Id", id),
					new MySqlParameter("@GameName", gameName)).Tables[0];

			return dt != null && dt.Rows.Count > 0 ? int.Parse(dt.Rows[0].ItemArray[0].ToString()) : 0;
		}

		public static DataTable GetTopPlayedChallenges(int top, string gameName = "")
		{
			const string Query =
				@"SELECT DISTINCT `cmd`.`Id`, `Title`, COUNT(*) AS Total
FROM `statistics`.`stratroulette_randomchallenge` `sc`
INNER JOIN `stratroulette`.`challenges` `cmd` ON `cmd`.`Id` = `ChallengeId`";

			var extraFilter = string.Empty;

			if (!string.IsNullOrEmpty(gameName))
			{
				extraFilter += " WHERE `GameName` = @GameName";
			}

			var groupBy = " GROUP BY  `cmd`.`Id`";
			var orderBy = " ORDER BY `Total` DESC LIMIT @Top";
			return Database.ExecuteQuery(Query + extraFilter + groupBy + orderBy, new MySqlParameter("@Top", top), new MySqlParameter("@GameName", gameName)).Tables[0];
		}

		public static DataTable GetTopLikedChallenges(int top, string gameName = "")
		{
			const string Query =
				@"SELECT DISTINCT `cmd`.`Id`, `Title`, (SELECT COUNT(*) FROM `stratroulette`.`feedback` WHERE `FeedbackKind` = 'Like' AND `ChallengeId` = `sc`.`Id`) AS `Total`
FROM `statistics`.`stratroulette_randomchallenge` `sc`
INNER JOIN `stratroulette`.`challenges` `cmd` ON `cmd`.`Id` = `sc`.`ChallengeId`";

			var extraFilter = string.Empty;

			if (!string.IsNullOrEmpty(gameName))
			{
				extraFilter += " WHERE `GameName` = @GameName";
			}

			var groupBy = " GROUP BY `cmd`.`Id`";
			var orderBy = " ORDER BY `Total` DESC LIMIT @Top";

			return Database.ExecuteQuery(Query + extraFilter + groupBy + orderBy, new MySqlParameter("@Top", top), new MySqlParameter("@GameName", gameName)).Tables[0];
		}

		public static DataTable GetTopPlayedDays(int top, string gameName = "")
		{
			const string Query =
				@"SELECT DISTINCT `cmd`.`Id`, `CreateDate` AS `Date`, COUNT(*) AS `Total`
FROM `statistics`.`stratroulette_randomchallenge` `sc`
INNER JOIN `stratroulette`.`challenges` `cmd` ON `cmd`.`Id` = `sc`.`ChallengeId`";

			var extraFilter = string.Empty;

			if (!string.IsNullOrEmpty(gameName))
			{
				extraFilter += " WHERE `GameName` = @GameName";
			}

			var groupBy = " GROUP BY `Date`";
			var orderBy = " ORDER BY `Total` DESC LIMIT @Top";

			return Database.ExecuteQuery(Query + extraFilter + groupBy + orderBy, new MySqlParameter("@Top", top), new MySqlParameter("@GameName", gameName)).Tables[0];
		}

		/* -----------------------------------------------------------------------
								Management Queries
		--------------------------------------------------------------------------*/
		public static DataTable ManagementLogin(string username, string password)
		{
			const string Query = @"SELECT * FROM `stratroulette`.`managementaccount` WHERE `Username` = @Username AND `Password` = @Password";

			var dt = Database.ExecuteQuery(Query, new MySqlParameter("@Username", username), new MySqlParameter("@Password", password)).Tables[0];

			return dt;
		}
	}
}