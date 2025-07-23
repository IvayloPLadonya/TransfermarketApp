using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Transfers
{
	public class TransferViewModel
	{
		public int TransferId { get; set; }
		public string PlayerName { get; set; } = null!;
		public string FromClubName { get; set; } = null!;
		public string ToClubName { get; set; } = null!;
		public decimal TransferFee { get; set; }
		public DateTime TransferDate { get; set; }
	}

}
