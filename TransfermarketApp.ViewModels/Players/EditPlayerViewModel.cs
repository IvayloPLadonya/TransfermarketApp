using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models.Enums;

namespace TransfermarketApp.ViewModels.Players
{
	public class EditPlayerViewModel
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

		[Required]
		public int? CurrentClubId { get; set; }

		public List<ClubDropdownViewModel> Clubs { get; set; } = new();
	}

}
