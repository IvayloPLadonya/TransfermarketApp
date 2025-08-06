using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Data.Models.Enums;
using TransfermarketApp.Services.Core;
using TransfermarketApp.ViewModels.Players;
using Xunit;
namespace TransfermarketApp.Tests
{
	public class PlayerServiceTests
	{
		private TransfermarketAppDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<TransfermarketAppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			var dbContext = new TransfermarketAppDbContext(options);

			var club = new Club { ClubId = 1, Name = "FC Test", ImageUrl = "club.png" };
			dbContext.Clubs.Add(club);

			dbContext.Players.AddRange(
				new Player { PlayerId = 1, Name = "Messi", Age = 36, MarketValue = 100000000, Position = Position.RW, CurrentClubId = 1, ImageUrl = "messi.png" },
				new Player { PlayerId = 2, Name = "Ronaldo", Age = 38, MarketValue = 50000000, Position = Position.LW, CurrentClubId = 1 }
			);

			dbContext.SaveChanges();
			return dbContext;
		}

		[Fact]
		public async Task GetAllPlayersAsync_ReturnsPaginatedResults()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetAllPlayersAsync(null, page: 1, pageSize: 1);

			Assert.Single(result);
			Assert.Equal("Messi", result.First().Name);
		}

		[Fact]
		public async Task GetPlayerByIdAsync_ReturnsCorrectPlayer()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetPlayerByIdAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Messi", result.Name);
		}

		[Fact]
		public async Task CreatePlayerAsync_AddsPlayer()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var newPlayer = new CreatePlayerViewModel
			{
				Name = "Xavi",
				Age = 40,
				MarketValue = 3000000,
				Position = Position.CM,
				ImageUrl = "xavi.png",
				CurrentClubId = 1
			};

			await service.CreatePlayerAsync(newPlayer);

			var player = await context.Players.FirstOrDefaultAsync(p => p.Name == "Xavi");
			Assert.NotNull(player);
			Assert.Equal(Position.CM, player.Position);
		}

		[Fact]
		public async Task UpdatePlayerAsync_ChangesValues()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var updated = new EditPlayerViewModel
			{
				PlayerId = 1,
				Name = "Leo Messi",
				Age = 37,
				MarketValue = 80000000,
				Position = Position.RW,
				ImageUrl = "new.png",
				CurrentClubId = 1
			};

			await service.UpdatePlayerAsync(1, updated);

			var result = await context.Players.FindAsync(1);
			Assert.Equal("Leo Messi", result.Name);
			Assert.Equal(37, result.Age);
		}

		[Fact]
		public async Task DeletePlayerAsync_RemovesPlayer()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			await service.DeletePlayerAsync(2);

			var player = await context.Players.FindAsync(2);
			Assert.Null(player);
		}

		[Fact]
		public async Task GetPlayersCountAsync_ReturnsCorrectCount()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var count = await service.GetPlayersCountAsync("Messi");
			Assert.Equal(1, count);
		}

		[Fact]
		public async Task GetFilteredPlayersAsync_ReturnsFilteredResults()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var filter = new PlayerFilterViewModel
			{
				SearchTerm = "Ronaldo",
				SortOrder = "value_desc"
			};

			var players = await service.GetFilteredPlayersAsync(filter, page: 1, pageSize: 10);
			Assert.Single(players);
			Assert.Equal("Ronaldo", players.First().Name);
		}
		[Fact]
		public async Task GetAllPlayersAsync_WithSearchTerm_FiltersCorrectly()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetAllPlayersAsync("Ronaldo", 1, 10);

			Assert.Single(result);
			Assert.Equal("Ronaldo", result.First().Name);
		}

		[Fact]
		public async Task GetPlayerByIdAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetPlayerByIdAsync(999);

			Assert.Null(result);
		}

		[Fact]
		public async Task GetPlayerForEditAsync_ValidId_ReturnsEditModel()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetPlayerForEditAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Messi", result.Name);
		}

		[Fact]
		public async Task GetPlayerForEditAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetPlayerForEditAsync(999);

			Assert.Null(result);
		}

		[Fact]
		public async Task UpdatePlayerAsync_InvalidId_ThrowsException()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var model = new EditPlayerViewModel
			{
				PlayerId = 999,
				Name = "Fake",
				Age = 30,
				MarketValue = 0,
				Position = Position.CM,
				ImageUrl = "fake.png",
				CurrentClubId = 1
			};

			await Assert.ThrowsAsync<Exception>(() => service.UpdatePlayerAsync(999, model));
		}

		[Fact]
		public async Task GetFilteredPlayersCountAsync_WithFilters_ReturnsCorrectCount()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var filter = new PlayerFilterViewModel
			{
				SearchTerm = "Messi",
				Age = 36,
				Position = Position.RW
			};

			var count = await service.GetFilteredPlayersCountAsync(filter);

			Assert.Equal(1, count);
		}

		[Fact]
		public async Task GetClubsAsync_ReturnsAllClubsOrderedByName()
		{
			var context = GetDbContext();
			var service = new PlayerService(context);

			var result = await service.GetClubsAsync();

			Assert.NotEmpty(result);
			Assert.Equal("FC Test", result.First().Name);
		}
	}
}
