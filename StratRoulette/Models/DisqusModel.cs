namespace StratRoulette.Models
{
	public class DisqusModel
	{
		public int Id { get; set; }

		public string WebpagePath { get; set; }

		public string Identifier { get; set; }

		public bool IsEmpty()
		{
			return (Id == 0 && string.IsNullOrEmpty(WebpagePath) && string.IsNullOrEmpty(Identifier));
		}
	}
}