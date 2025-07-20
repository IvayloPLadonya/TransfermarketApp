using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.ViewModels.Players;

namespace TransfermarketApp.ViewModels.Clubs
{
	public class ClubDetailsViewModel
	{
		public int ClubId { get; set; }
		public string Name { get; set; } = null!;
		public int FoundedYear { get; set; }
		public decimal Budget { get; set; }
		public string? ImageUrl { get; set; }
		public string LeagueName { get; set; } = null!;

		public List<PlayerViewModel> Players { get; set; } = new();
		public List<TransferViewModel> IncomingTransfers { get; set; } = new();
		public List<TransferViewModel> OutgoingTransfers { get; set; } = new();
	}
}
