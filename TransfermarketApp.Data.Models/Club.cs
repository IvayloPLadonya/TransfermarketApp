namespace TransfermarketApp.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Club
	{
		public int ClubId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; } = null!;
		[Required, Range(1800, 2100)]
		public int FoundedYear { get; set; }
		[Required, Range(0, double.MaxValue)]
		public decimal Budget { get; set; }

		[Url]
		public string? ImageUrl { get; set; }
		[Required]
		public int LeagueId { get; set; }
		[ForeignKey(nameof(LeagueId))]
		public League League { get; set; } = null!;

		public ICollection<Player> Players { get; set; } = new List<Player>();
	}

}