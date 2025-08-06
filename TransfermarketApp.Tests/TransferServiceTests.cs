using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core;
using TransfermarketApp.ViewModels.Transfers;
using Xunit;

namespace TransfermarketApp.Tests
{
	public class TransferServiceTests
	{
		private TransfermarketAppDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<TransfermarketAppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new TransfermarketAppDbContext(options);

			var club1 = new Club { ClubId = 1, Name = "Barcelona" };
			var club2 = new Club { ClubId = 2, Name = "PSG" };
			var player = new Player { PlayerId = 1, Name = "Neymar", ImageUrl = "neymar.png", CurrentClubId = 1 };

			var transfer = new Transfer
			{
				TransferId = 1,
				Player = player,
				FromClub = club1,
				ToClub = club2,
				TransferFee = 222000000,
				TransferDate = new DateTime(2024, 1, 15)
			};

			context.Clubs.AddRange(club1, club2);
			context.Players.Add(player);
			context.Transfers.Add(transfer);
			context.SaveChanges();

			return context;
		}

		[Fact]
		public async Task GetAllTransfersAsync_ReturnsTransfers()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var result = await service.GetAllTransfersAsync(new TransferFilterViewModel());

			Assert.Single(result);
			Assert.Equal("Neymar", result.First().PlayerName);
		}

		[Fact]
		public async Task CreateTransferAsync_AddsTransfer()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var model = new CreateTransferViewModel
			{
				PlayerId = 1,
				FromClubId = 2,
				ToClubId = 1,
				TransferFee = 50000000,
				TransferDate = new DateTime(2024, 5, 1)
			};

			await service.CreateTransferAsync(model);

			var transfers = await context.Transfers.ToListAsync();
			Assert.Equal(2, transfers.Count);
			Assert.Contains(transfers, t => t.TransferFee == 50000000);
		}

		[Fact]
		public async Task GetTransferForEditAsync_ValidId_ReturnsModel()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var model = await service.GetTransferForEditAsync(1);

			Assert.NotNull(model);
			Assert.Equal(1, model.TransferId);
		}

		[Fact]
		public async Task GetTransferForEditAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var model = await service.GetTransferForEditAsync(999);

			Assert.Null(model);
		}

		[Fact]
		public async Task UpdateTransferAsync_ValidId_UpdatesTransfer()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var model = new EditTransferViewModel
			{
				TransferId = 1,
				PlayerId = 1,
				FromClubId = 2,
				ToClubId = 1,
				TransferFee = 333000000,
				TransferDate = DateTime.Now
			};

			await service.UpdateTransferAsync(model);

			var updated = await context.Transfers.FindAsync(1);
			Assert.Equal(333000000, updated.TransferFee);
		}

		[Fact]
		public async Task DeleteTransferAsync_RemovesTransfer()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			await service.DeleteTransferAsync(1);

			var transfer = await context.Transfers.FindAsync(1);
			Assert.Null(transfer);
		}

		[Fact]
		public async Task DeleteTransferAsync_InvalidId_DoesNothing()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			await service.DeleteTransferAsync(999);

			var count = await context.Transfers.CountAsync();
			Assert.Equal(1, count);
		}

		[Fact]
		public async Task GetFilteredTransfersAsync_ByClubName_Works()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var filter = new TransferFilterViewModel { ClubName = "Barcelona" };
			var results = await service.GetFilteredTransfersAsync(filter, 1, 10);

			Assert.Single(results);
		}

		[Fact]
		public async Task GetFilteredTransfersCountAsync_ByPlayerName_ReturnsCorrectCount()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var filter = new TransferFilterViewModel { PlayerName = "Neymar" };
			var count = await service.GetFilteredTransfersCountAsync(filter);

			Assert.Equal(1, count);
		}

		[Fact]
		public async Task GetCreateModelAsync_ReturnsDropdowns()
		{
			var context = GetDbContext();
			var service = new TransferService(context);

			var result = await service.GetCreateModelAsync();

			Assert.NotEmpty(result.Clubs);
			Assert.NotEmpty(result.Players);
		}
	}
}
