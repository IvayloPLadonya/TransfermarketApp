using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.Players;

namespace TransfermarketApp.Services.Core
{
	public class PlayerService : IPlayerService
	{
		private readonly TransfermarketAppDbContext _dbContext;

		public PlayerService(TransfermarketAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<PlayerListViewModel>> GetAllPlayersAsync(string? searchTerm, int page, int pageSize)
		{
			var query = _dbContext.Players
				.Include(p => p.CurrentClub)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				query = query.Where(p => p.Name.Contains(searchTerm));
			}

			return await query
				.OrderBy(p => p.Name)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(p => new PlayerListViewModel
				{
					PlayerId = p.PlayerId,
					Name = p.Name,
					Position = p.Position.ToString(),
					Age = p.Age,
					MarketValue = p.MarketValue,
					CurrentClubName = p.CurrentClub.Name,
					ImageUrl = p.ImageUrl
				})
				.ToListAsync();
		}

		public async Task<PlayerDetailsViewModel?> GetPlayerByIdAsync(int id)
		{
			return await _dbContext.Players
				.Where(p => p.PlayerId == id)
				.Include(p => p.CurrentClub)
				.Select(p => new PlayerDetailsViewModel
				{
					PlayerId = p.PlayerId,
					Name = p.Name,
					Position = p.Position.ToString(),
					Age = p.Age,
					MarketValue = p.MarketValue,
					ImageUrl = p.ImageUrl,
					CurrentClubName = p.CurrentClub.Name
				})
				.FirstOrDefaultAsync();
		}

		public async Task CreatePlayerAsync(CreatePlayerViewModel model)
		{
			var player = new Player
			{
				Name = model.Name,
				Position = model.Position,
				Age = model.Age,
				MarketValue = model.MarketValue,
				ImageUrl = model.ImageUrl,
				CurrentClubId = model.CurrentClubId
			};

			_dbContext.Players.Add(player);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdatePlayerAsync(int id, EditPlayerViewModel model)
		{
			var player = await _dbContext.Players.FindAsync(id);
			if (player == null) throw new Exception("Player not found");

			player.Name = model.Name;
			player.Position = model.Position;
			player.Age = model.Age;
			player.MarketValue = model.MarketValue;
			player.ImageUrl = model.ImageUrl;
			player.CurrentClubId = model.CurrentClubId;

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeletePlayerAsync(int id)
		{
			var player = await _dbContext.Players.FindAsync(id);
			if (player != null)
			{
				_dbContext.Players.Remove(player);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<int> GetPlayersCountAsync(string? searchTerm)
		{
			var query = _dbContext.Players.AsQueryable();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				query = query.Where(p => p.Name.Contains(searchTerm));
			}
			return await query.CountAsync();
		}
	}
}
