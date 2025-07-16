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
		Task UpdatePlayerAsync(int id,EditPlayerViewModel player);
		Task DeletePlayerAsync(int id);
		Task<int> GetPlayersCountAsync(string? searchTerm = null);
	}
}
