using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Players
{
	public class PlayerDetailsViewModel
	{
		public int PlayerId { get; set; }
		public string Name { get; set; } = null!;
		public string Position { get; set; } = null!;
		public int Age { get; set; }
		public decimal MarketValue { get; set; }
		public string? ImageUrl { get; set; }
		public int CurrentClubId { get; set; }
		public List<PlayerStatViewModel> Stats { get; set; } = new();
		public List<TransferViewModel> Transfers { get; set; } = new();
	}
}
