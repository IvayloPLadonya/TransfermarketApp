// ViewModels/Players/PlayerFilterViewModel.cs
using System.Collections.Generic;
using TransfermarketApp.Data.Models.Enums;

namespace TransfermarketApp.ViewModels.Players
{
	public class PlayerFilterViewModel
	{
		public string? SearchTerm { get; set; }
		public int? ClubId { get; set; }
		public Position? Position { get; set; }
		public int? Age { get; set; }
		public string? SortOrder { get; set; }

		public List<ClubDropdownViewModel> Clubs { get; set; } = new();
	}
}
