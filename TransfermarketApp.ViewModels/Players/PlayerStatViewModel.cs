using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransfermarketApp.ViewModels.Players
{
		public class PlayerStatViewModel
		{
			public int StatId { get; set; }
			public string Season { get; set; } = null!;
			public string Club { get; set; } = null!;
			public int Appearances { get; set; }
			public int Goals { get; set; }
			public int Assists { get; set; }
		}

}
