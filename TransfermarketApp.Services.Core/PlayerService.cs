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
			var player = await _dbContext.Players
		.Include(p => p.CurrentClub)
		.Include(p => p.PlayerStats)
			.ThenInclude(s => s.Club)
		.Include(p => p.Transfers)
			.ThenInclude(t => t.FromClub)
		.Include(p => p.Transfers)
			.ThenInclude(t => t.ToClub)
		.FirstOrDefaultAsync(p => p.PlayerId == id);

			if (player == null)
			{
				return null;
			}

			return new PlayerDetailsViewModel
			{
				PlayerId = player.PlayerId,
				Name = player.Name,
				Position = player.Position.ToString(),
				Age = player.Age,
				MarketValue = player.MarketValue,
				ImageUrl = player.ImageUrl,
				CurrentClubId = player.CurrentClub.ClubId,
				CurrentClubName = player.CurrentClub?.Name,
				CurrentClubLogoUrl = player.CurrentClub?.ImageUrl,

				Stats = player.PlayerStats
					.Select(s => new PlayerStatViewModel
					{
						StatId = s.StatId,
						Season = s.Season,
						Club=s.Club.Name,
						Appearances = s.Appearances,
						Goals = s.Goals,
						Assists = s.Assists
					}).ToList(),

				Transfers = player.Transfers
					.Select(t => new TransferViewModel
					{
						FromClubName = t.FromClub.Name,
						ToClubName = t.ToClub.Name,
						TransferFee = t.TransferFee,
						TransferDate = t.TransferDate
					}).ToList()
			};
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

		public async Task<EditPlayerViewModel?> GetPlayerForEditAsync(int id)
		{
			var player = await _dbContext.Players
				.Include(p => p.CurrentClub)
				.FirstOrDefaultAsync(p => p.PlayerId == id);

			if (player == null) return null;

			return new EditPlayerViewModel
			{
				PlayerId = player.PlayerId,
				Name = player.Name,
				Position = player.Position,
				Age = player.Age,
				MarketValue = player.MarketValue,
				ImageUrl = player.ImageUrl,
				CurrentClubId = player.CurrentClubId
			};
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
		public async Task<List<ClubDropdownViewModel>> GetClubsAsync()
		{
			return await _dbContext.Clubs
				.OrderBy(c => c.Name)
				.Select(c => new ClubDropdownViewModel
				{
					Id = c.ClubId,
					Name = c.Name
				})
				.ToListAsync();
		}
		public async Task<IEnumerable<PlayerListViewModel>> GetFilteredPlayersAsync(PlayerFilterViewModel filter, int page, int pageSize)
		{
			var query = _dbContext.Players
				.Include(p => p.CurrentClub)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
				query = query.Where(p => p.Name.Contains(filter.SearchTerm));

			if (filter.ClubId.HasValue)
				query = query.Where(p => p.CurrentClubId == filter.ClubId);

			if (filter.Position.HasValue)
				query = query.Where(p => p.Position == filter.Position);

			if (filter.Age.HasValue)
				query = query.Where(p => p.Age == filter.Age);

			query = filter.SortOrder switch
			{
				"value_asc" => query.OrderBy(p => p.MarketValue),
				"value_desc" => query.OrderByDescending(p => p.MarketValue),
				_ => query.OrderBy(p => p.Name)
			};

			return await query
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

		public async Task<int> GetFilteredPlayersCountAsync(PlayerFilterViewModel filter)
		{
			var query = _dbContext.Players.AsQueryable();

			if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
				query = query.Where(p => p.Name.Contains(filter.SearchTerm));

			if (filter.ClubId.HasValue)
				query = query.Where(p => p.CurrentClubId == filter.ClubId);

			if (filter.Position.HasValue)
				query = query.Where(p => p.Position == filter.Position);

			if (filter.Age.HasValue)
				query = query.Where(p => p.Age == filter.Age);

			return await query.CountAsync();
		}


	}
}
