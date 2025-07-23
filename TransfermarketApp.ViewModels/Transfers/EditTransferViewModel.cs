using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Transfers
{
	public class EditTransferViewModel
	{
		public int TransferId { get; set; }
		public int PlayerId { get; set; }
		public int FromClubId { get; set; }
		public int ToClubId { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TransferFee { get; set; }

		[DataType(DataType.Date)]
		public DateTime TransferDate { get; set; }

		public List<ClubDropdownViewModel> Clubs { get; set; } = new();
		public List<PlayerDropdownViewModel> Players { get; set; } = new();
	}
}
