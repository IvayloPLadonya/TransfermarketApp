using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.PlayerStats
{
		public class CreatePlayerStatViewModel
		{
			[Required]
			public int PlayerId { get; set; }

			[Required]
			public int ClubId { get; set; }

			[Required]
			[StringLength(20)]
			public string Season { get; set; } = null!;

			[Range(0, 100)]
			public int Appearances { get; set; }

			[Range(0, 100)]
			public int Goals { get; set; }

			[Range(0, 100)]
			public int Assists { get; set; }
			public List<ClubDropdownViewModel> Clubs { get; set; } = new();
	}
	}
