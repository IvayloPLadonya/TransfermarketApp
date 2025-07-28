using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models;
using TransfermarketApp.ViewModels.Players;

namespace TransfermarketApp.Services.Core.Contracts
{
	public interface IPlayerService
	{
		Task<IEnumerable<PlayerListViewModel>> GetAllPlayersAsync(string? searchTerm = null, int page = 1, int pageSize = 10);
		Task<PlayerDetailsViewModel?> GetPlayerByIdAsync(int id);
		Task CreatePlayerAsync(CreatePlayerViewModel player);
		Task<EditPlayerViewModel?> GetPlayerForEditAsync(int id);
		Task UpdatePlayerAsync(int id,EditPlayerViewModel player);
		Task DeletePlayerAsync(int id);
		Task<int> GetPlayersCountAsync(string? searchTerm = null);
		Task<List<ClubDropdownViewModel>> GetClubsAsync();
		Task<int> GetFilteredPlayersCountAsync(PlayerFilterViewModel filter);
		Task<IEnumerable<PlayerListViewModel>> GetFilteredPlayersAsync(PlayerFilterViewModel filter, int page, int pageSize);
		}
}
