using Microsoft.EntityFrameworkCore;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.Clubs;

namespace TransfermarketApp.Services.Core
{
	public class ClubService : IClubService
	{
		private readonly TransfermarketAppDbContext _dbContext;

		public ClubService(TransfermarketAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<ClubListViewModel>> GetAllClubsAsync()
		{
			return await _dbContext.Clubs
				.Include(c => c.League)
				.OrderBy(c => c.Name)
				.Select(c => new ClubListViewModel
				{
					ClubId = c.ClubId,
					Name = c.Name,
					FoundedYear = c.FoundedYear,
					Budget = c.Budget,
					ImageUrl = c.ImageUrl,
					LeagueName = c.League.Name
				})
				.ToListAsync();
		}

		public async Task<ClubDetailsViewModel?> GetClubByIdAsync(int id)
		{
			var club = await _dbContext.Clubs
	.Include(c => c.League)
	.Include(c => c.Players)
	.Include(c => c.IncomingTransfers)
		.ThenInclude(t => t.Player)
	.Include(c => c.IncomingTransfers)
		.ThenInclude(t => t.FromClub)
	.Include(c => c.OutgoingTransfers)
		.ThenInclude(t => t.Player)
	.Include(c => c.OutgoingTransfers)
		.ThenInclude(t => t.ToClub)
	.FirstOrDefaultAsync(c => c.ClubId == id);


			if (club == null) return null;

			return new ClubDetailsViewModel
			{
				ClubId = club.ClubId,
				Name = club.Name,
				FoundedYear = club.FoundedYear,
				Budget = club.Budget,
				ImageUrl = club.ImageUrl,
				LeagueName = club.League.Name,

				Players = club.Players.Select(p => new PlayerViewModel
				{
					Id = p.PlayerId,
					Name = p.Name,
					Position = p.Position.ToString(),
					Age = p.Age,
					MarketValue = p.MarketValue
				}).ToList(),

				IncomingTransfers = club.IncomingTransfers.Select(t => new TransferViewModel
				{
					PlayerName = t.Player.Name,
					FromClubName = t.FromClub.Name,
					Fee = t.TransferFee,
					TransferDate = t.TransferDate
				}).ToList(),

				OutgoingTransfers = club.OutgoingTransfers.Select(t => new TransferViewModel
				{
					PlayerName = t.Player.Name,
					ToClubName = t.ToClub.Name,
					Fee = t.TransferFee,
					TransferDate = t.TransferDate
				}).ToList()
			};
		}



		public async Task CreateClubAsync(CreateClubViewModel model)
		{
			var club = new Club
			{
				Name = model.Name,
				FoundedYear = model.FoundedYear,
				Budget = model.Budget,
				ImageUrl = model.ImageUrl,
				LeagueId = model.LeagueId
			};

			_dbContext.Clubs.Add(club);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<EditClubViewModel?> GetClubForEditAsync(int id)
		{
			var club = await _dbContext.Clubs.FindAsync(id);
			if (club == null) return null;

			return new EditClubViewModel
			{
				ClubId = club.ClubId,
				Name = club.Name,
				FoundedYear = club.FoundedYear,
				Budget = club.Budget,
				ImageUrl = club.ImageUrl,
				LeagueId = club.LeagueId
			};
		}

		public async Task UpdateClubAsync(int id, EditClubViewModel model)
		{
			var club = await _dbContext.Clubs.FindAsync(id);
			if (club == null) throw new Exception("Club not found.");

			club.Name = model.Name;
			club.FoundedYear = model.FoundedYear;
			club.Budget = model.Budget;
			club.ImageUrl = model.ImageUrl;
			club.LeagueId = model.LeagueId;

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteClubAsync(int id)
		{
			var club = await _dbContext.Clubs.FindAsync(id);
			if (club != null)
			{
				_dbContext.Clubs.Remove(club);
				await _dbContext.SaveChangesAsync();
			}
		}
		public async Task<List<LeagueDropdownViewModel>> GetLeaguesAsync()
		{
			return await _dbContext.Leagues
				.OrderBy(l => l.Name)
				.Select(l => new LeagueDropdownViewModel
				{
					Id = l.LeagueId,
					Name = l.Name
				})
				.ToListAsync();
		}

	}
}
