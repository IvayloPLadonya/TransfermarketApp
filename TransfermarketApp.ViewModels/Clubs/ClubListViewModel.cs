using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Clubs
{
	public class ClubListViewModel
	{
		public int ClubId { get; set; }

		public string Name { get; set; } = null!;

		public int FoundedYear { get; set; }

		public decimal Budget { get; set; }

		public string? ImageUrl { get; set; }

		public string LeagueName { get; set; } = null!;
		public int LeagueId { get; set; }
	}
}
