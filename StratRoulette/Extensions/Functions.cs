namespace StratRoulette.Extensions
{
	using System.IO;
	using System.Web;

	public static class Functions
	{
		public static string GetChallengeColor(string side)
		{
			switch (side?.ToLower())
			{
			case "defend":
				return "warning";
			case "attack":
				return "primary";
			case "both":
				return "default";
			default:
				return "default";
			}
		}

		public static int GetDifficultyInt(string difficulty)
		{
			switch (difficulty?.ToLower())
			{
			case "easy":
				return 1;
			case "medium":
				return 2;
			case "hard":
				return 3;
			default:
				return 0;
			}
		}

	    public static string GetVersion()
	    {
			return File.ReadAllText(HttpContext.Current.Server.MapPath("~/version.txt"));
		}
	}
}
