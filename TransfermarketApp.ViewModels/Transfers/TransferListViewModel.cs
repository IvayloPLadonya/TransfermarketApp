using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Transfers
{
	public class TransferListViewModel
	{
		public IEnumerable<TransferViewModel> Transfers { get; set; } = new List<TransferViewModel>();
		public TransferFilterViewModel Filter { get; set; } = new();

		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
	}

}
