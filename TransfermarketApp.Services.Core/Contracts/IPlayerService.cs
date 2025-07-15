using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models;

namespace TransfermarketApp.Services.Core.Contracts
{
	public interface IPlayerService
	{
		Task<IEnumerable<Player>> GetAllPlayersAsync(string? searchTerm = null, int page = 1, int pageSize = 10);
		Task<Player?> GetPlayerByIdAsync(int id);
		Task CreatePlayerAsync(Player player);
		Task UpdatePlayerAsync(Player player);
		Task DeletePlayerAsync(int id);
		Task<int> GetPlayersCountAsync(string? searchTerm = null);
	}
}
