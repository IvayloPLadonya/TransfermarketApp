using System;
namespace TransfermarketApp.Data.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Transfer
	{
		public int TransferId { get; set; }

		public int PlayerId { get; set; }
		public Player Player { get; set; } = null!;

		public int FromClubId { get; set; }
		public Club FromClub { get; set; } = null!;

		public int ToClubId { get; set; }
		public Club ToClub { get; set; } = null!;

		[Range(0, double.MaxValue)]
		public decimal TransferFee { get; set; }

		[DataType(DataType.Date)]
		public DateTime TransferDate { get; set; }
	}

}
