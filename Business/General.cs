namespace Business
{
	using System;
	using System.Collections.Generic;

	using Extensions;

	using Models;

	public class General
	{
		public static IList<BusinessVideoModel> GetAllVideos()
		{
			return DatabaseCommunication.GetAllVideos().ToList<BusinessVideoModel>();
		}

		public static IList<BusinessMapModel> GetAllMaps(string gameName)
		{
			var maps = DatabaseCommunication.GetAllMaps(gameName).ToList<BusinessMapModel>();
			return maps;
		}

		public static IList<BusinessGameModeModel> GetAllGameModes(string gameName)
		{
			var gameModes = DatabaseCommunication.GetAllGameModes(gameName).ToList<BusinessGameModeModel>();

			return gameModes;
		}

		public static void AddLog(Exception ex)
		{
			DatabaseCommunication.AddLog(ex);
		}
	}
}