using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.ViewModels.PlayerStats;

namespace TransfermarketApp.Services.Core.Contracts
{
	public interface IPlayerStatService
	{
		Task AddStatAsync(CreatePlayerStatViewModel model);
		Task UpdateStatAsync(EditPlayerStatViewModel model);
		Task DeleteStatAsync(int statId);
		Task<EditPlayerStatViewModel?> GetStatByIdAsync(int statId);
		Task<List<ClubDropdownViewModel>> GetClubsAsync();
	}
}
