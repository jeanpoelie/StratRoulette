namespace StratRoulette.Models
{
	using System;

	public class BusinessStatisticModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public DateTime? Date { get; set; }

		public long Total { get; set; }
	}
}