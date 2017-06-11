namespace ModelLibrary
{
	using System;
	using System.Collections.Generic;

    using Business;
    using Business.Extensions;

	using ModelLibrary.Models;

    public class General
	{
		public static IList<VideoModel> GetAllVideos()
		{
			return DatabaseCommunication.GetAllVideos().ToList<VideoModel>();
		}

		public static IList<MapModel> GetAllMaps(string gameName)
		{
			var maps = DatabaseCommunication.GetAllMaps(gameName).ToList<MapModel>();
			return maps;
		}

		public static IList<GameModeModel> GetAllGameModes(string gameName)
		{
			var gameModes = DatabaseCommunication.GetAllGameModes(gameName).ToList<GameModeModel>();

			return gameModes;
		}

		public static void AddLog(Exception ex)
		{
			DatabaseCommunication.AddLog(ex);
		}
	}
}