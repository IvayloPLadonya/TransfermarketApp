using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Data;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.PlayerStats;
using Microsoft.EntityFrameworkCore;

namespace TransfermarketApp.Services.Core
{
	public class PlayerStatService : IPlayerStatService
	{
		private readonly TransfermarketAppDbContext _context;

		public PlayerStatService(TransfermarketAppDbContext context)
		{
			_context = context;
		}

		public async Task AddStatAsync(CreatePlayerStatViewModel model)
		{
			var entity = new PlayerStat
			{
				PlayerId = model.PlayerId,
				ClubId = model.ClubId,
				Season = model.Season,
				Appearances = model.Appearances,
				Goals = model.Goals,
				Assists = model.Assists
			};

			_context.PlayerStats.Add(entity);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateStatAsync(EditPlayerStatViewModel model)
		{
			var stat = await _context.PlayerStats.FindAsync(model.StatId);
			if (stat == null) return;

			stat.PlayerId = model.PlayerId;
			stat.ClubId = model.ClubId;
			stat.Season = model.Season;
			stat.Appearances = model.Appearances;
			stat.Goals = model.Goals;
			stat.Assists = model.Assists;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteStatAsync(int statId)
		{
			var stat = await _context.PlayerStats.FindAsync(statId);
			if (stat == null) return;

			_context.PlayerStats.Remove(stat);
			await _context.SaveChangesAsync();
		}

		public async Task<EditPlayerStatViewModel?> GetStatByIdAsync(int statId)
		{
			var stat = await _context.PlayerStats
				.Where(ps => ps.StatId == statId)
				.Select(ps => new EditPlayerStatViewModel
				{
					StatId = ps.StatId,
					PlayerId = ps.PlayerId,
					ClubId = ps.ClubId,
					Season = ps.Season,
					Appearances = ps.Appearances,
					Goals = ps.Goals,
					Assists = ps.Assists
				})
				.FirstOrDefaultAsync();

			if (stat == null) return null;

			stat.Clubs = await GetClubsAsync();

			return stat;
		}

		public async Task<List<ClubDropdownViewModel>> GetClubsAsync()
		{
			return await _context.Clubs
				.OrderBy(c => c.Name)
				.Select(c => new ClubDropdownViewModel
				{
					Id = c.ClubId,
					Name = c.Name
				})
				.ToListAsync();
		}

	}
}
