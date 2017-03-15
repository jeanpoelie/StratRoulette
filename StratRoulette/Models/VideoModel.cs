namespace StratRoulette.Models
{
	using System;

	public enum VideoType
	{
		Youtube,

		Twitch
	}

	public class VideoModel
	{
		public int Id { get; set; }

		public string VideoType { get; set; }

		public string Title { get; set; }

		public string Url { get; set; }

		public DateTime Date { get; set; }
	}
}