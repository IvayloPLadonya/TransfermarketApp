using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Clubs
{
	public class TransferViewModel
	{
		public int Id { get; set; }
		public string PlayerName { get; set; } = null!;
		public string FromClubName { get; set; } = null!;
		public string ToClubName { get; set; } = null!;
		public decimal Fee { get; set; }
		public DateTime TransferDate { get; set; }
	}

}
