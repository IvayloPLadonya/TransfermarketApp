using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.Data.Models
{
	using System.ComponentModel.DataAnnotations;

	public class PlayerStat
	{
		[Key]
		public int StatId { get; set; }

		public int PlayerId { get; set; }
		public Player Player { get; set; } = null!;

		[Required, StringLength(20)]
		public string Season { get; set; } = null!;

		[Range(0, 100)]
		public int Appearances { get; set; }

		[Range(0, 100)]
		public int Goals { get; set; }

		[Range(0, 100)]
		public int Assists { get; set; }
		[Required]
		public int ClubId { get; set; }
		public Club Club { get; set; } = null!;

	}

}
