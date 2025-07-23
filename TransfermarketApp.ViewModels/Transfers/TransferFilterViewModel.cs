using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Transfers
{
	public class TransferFilterViewModel
	{
		public string? PlayerName { get; set; }
		public string? ClubName { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
	}

}
