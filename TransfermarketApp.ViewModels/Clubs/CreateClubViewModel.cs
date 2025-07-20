using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Clubs
{
	public class CreateClubViewModel
	{
		[Required]
		[StringLength(100, MinimumLength = 2)]
		public string Name { get; set; } = null!;

		[Required]
		[Range(1800, 2100)]
		public int FoundedYear { get; set; }

		[Required]
		[Range(0, double.MaxValue)]
		public decimal Budget { get; set; }

		[Url]
		public string? ImageUrl { get; set; }

		[Required]
		[Display(Name = "League")]
		public int LeagueId { get; set; }
		public List<LeagueDropdownViewModel> Leagues { get; set; } = new List<LeagueDropdownViewModel>();
	}
}
