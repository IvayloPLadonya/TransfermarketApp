namespace TransfermarketApp.Data.Models
{
	using System.ComponentModel.DataAnnotations;
	using TransfermarketApp.Data.Models.Enums;

	public class Player
	{
		public int PlayerId { get; set; }

		[Required, StringLength(100)]
		public string Name { get; set; } = null!;

		[Required]
		public Position Position { get; set; }

		[Range(15, 50)]
		public int Age { get; set; }

		[Range(0, double.MaxValue)]
		public decimal MarketValue { get; set; }

		[Url]
		public string? ImageUrl { get; set; }

		public int CurrentClubId { get; set; }
		public Club CurrentClub { get; set; } = null!;

		public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
		public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();
	}

}
