using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Clubs
{
	public class PlayerViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Position { get; set; } = null!;
		public int Age { get; set; }
		public decimal MarketValue { get; set; }
	}
}
