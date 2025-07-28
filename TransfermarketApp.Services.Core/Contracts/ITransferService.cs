using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.ViewModels.Transfers;

namespace TransfermarketApp.Services.Core.Contracts
{
	public interface ITransferService
	{
		Task<List<TransferViewModel>> GetAllTransfersAsync(TransferFilterViewModel filter);
		Task<CreateTransferViewModel> GetCreateModelAsync();
		Task CreateTransferAsync(CreateTransferViewModel model);
		Task<EditTransferViewModel?> GetTransferForEditAsync(int id);
		Task UpdateTransferAsync(EditTransferViewModel model);
		Task DeleteTransferAsync(int id);
		Task<IEnumerable<TransferViewModel>> GetFilteredTransfersAsync(TransferFilterViewModel filter, int page, int pageSize);
		Task<int> GetFilteredTransfersCountAsync(TransferFilterViewModel filter);

	}
}
