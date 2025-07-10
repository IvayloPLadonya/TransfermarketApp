namespace TransfermarketApp.Data.Models
{
	using System.ComponentModel.DataAnnotations;

	public class League
	{
		public int LeagueId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; } = null!;

		[Required, StringLength(100)]
		public string Country { get; set; } = null!;

		[Required, StringLength(50)]
		public string Level { get; set; } = null!; 

		[Url]
		public string? ImageUrl { get; set; }

		public ICollection<Club> Clubs { get; set; } = new List<Club>();
	}

}
