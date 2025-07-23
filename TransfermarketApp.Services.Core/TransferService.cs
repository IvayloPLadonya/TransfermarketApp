using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Data;
using TransfermarketApp.ViewModels.Transfers;
using Microsoft.EntityFrameworkCore;
using TransfermarketApp.Services.Core.Contracts;

namespace TransfermarketApp.Services.Core
{
	public class TransferService : ITransferService
	{
		private readonly TransfermarketAppDbContext _dbContext;

		public TransferService(TransfermarketAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<TransferViewModel>> GetAllTransfersAsync(TransferFilterViewModel filter)
		{
			var query = _dbContext.Transfers
				.Include(t => t.Player)
				.Include(t => t.FromClub)
				.Include(t => t.ToClub)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(filter.PlayerName))
				query = query.Where(t => t.Player.Name.Contains(filter.PlayerName));

			if (!string.IsNullOrWhiteSpace(filter.ClubName))
				query = query.Where(t => t.FromClub.Name.Contains(filter.ClubName) || t.ToClub.Name.Contains(filter.ClubName));

			if (filter.FromDate.HasValue)
				query = query.Where(t => t.TransferDate >= filter.FromDate.Value);

			if (filter.ToDate.HasValue)
				query = query.Where(t => t.TransferDate <= filter.ToDate.Value);

			return await query
				.OrderByDescending(t => t.TransferDate)
				.Select(t => new TransferViewModel
				{
					TransferId = t.TransferId,
					PlayerName = t.Player.Name,
					FromClubName = t.FromClub.Name,
					ToClubName = t.ToClub.Name,
					TransferFee = t.TransferFee,
					TransferDate = t.TransferDate
				}).ToListAsync();
		}

		public async Task<CreateTransferViewModel> GetCreateModelAsync()
		{
			return new CreateTransferViewModel
			{
				Clubs = await GetClubsAsync(),
				Players = await GetPlayersAsync()
			};
		}

		public async Task CreateTransferAsync(CreateTransferViewModel model)
		{
			var transfer = new Transfer
			{
				PlayerId = model.PlayerId,
				FromClubId = model.FromClubId,
				ToClubId = model.ToClubId,
				TransferFee = model.TransferFee,
				TransferDate = model.TransferDate
			};

			_dbContext.Transfers.Add(transfer);

			var player = await _dbContext.Players.FindAsync(model.PlayerId);
			if (player != null)
			{
				player.CurrentClubId = model.ToClubId;
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task<EditTransferViewModel?> GetTransferForEditAsync(int id)
		{
			var transfer = await _dbContext.Transfers.FindAsync(id);
			if (transfer == null) return null;

			return new EditTransferViewModel
			{
				TransferId = transfer.TransferId,
				PlayerId = transfer.PlayerId,
				FromClubId = transfer.FromClubId,
				ToClubId = transfer.ToClubId,
				TransferFee = transfer.TransferFee,
				TransferDate = transfer.TransferDate,
				Clubs = await GetClubsAsync(),
				Players = await GetPlayersAsync()
			};
		}

		public async Task UpdateTransferAsync(EditTransferViewModel model)
		{
			var transfer = await _dbContext.Transfers.FindAsync(model.TransferId);
			if (transfer == null) return;

			transfer.PlayerId = model.PlayerId;
			transfer.FromClubId = model.FromClubId;
			transfer.ToClubId = model.ToClubId;
			transfer.TransferFee = model.TransferFee;
			transfer.TransferDate = model.TransferDate;

			var player = await _dbContext.Players.FindAsync(model.PlayerId);
			if (player != null)
			{
				player.CurrentClubId = model.ToClubId;
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteTransferAsync(int id)
		{
			var transfer = await _dbContext.Transfers.FindAsync(id);
			if (transfer != null)
			{
				_dbContext.Transfers.Remove(transfer);
				await _dbContext.SaveChangesAsync();
			}
		}

		private async Task<List<ClubDropdownViewModel>> GetClubsAsync()
		{
			return await _dbContext.Clubs
				.OrderBy(c => c.Name)
				.Select(c => new ClubDropdownViewModel
				{
					Id = c.ClubId,
					Name = c.Name
				}).ToListAsync();
		}

		private async Task<List<PlayerDropdownViewModel>> GetPlayersAsync()
		{
			return await _dbContext.Players
				.OrderBy(p => p.Name)
				.Select(p => new PlayerDropdownViewModel
				{
					Id = p.PlayerId,
					Name = p.Name
				}).ToListAsync();
		}
	}
}
