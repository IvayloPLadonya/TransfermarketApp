using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Players
{
	public class TransferViewModel
	{
		public string FromClubName { get; set; } = null!;
		public string ToClubName { get; set; } = null!;
		public decimal TransferFee { get; set; }
		public DateTime TransferDate { get; set; }
	}
}
