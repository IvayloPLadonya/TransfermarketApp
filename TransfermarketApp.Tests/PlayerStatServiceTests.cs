using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core;
using TransfermarketApp.ViewModels.PlayerStats;
using Xunit;

namespace TransfermarketApp.Tests
{
	public class PlayerStatServiceTests
	{
		private TransfermarketAppDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<TransfermarketAppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new TransfermarketAppDbContext(options);

			var club = new Club { ClubId = 1, Name = "Juventus" };
			var player = new Player { PlayerId = 1, Name = "Cristiano Ronaldo", CurrentClubId = 1 };
			var stat = new PlayerStat
			{
				StatId = 1,
				PlayerId = 1,
				ClubId = 1,
				Season = "2023/24",
				Appearances = 30,
				Goals = 25,
				Assists = 5
			};

			context.Clubs.Add(club);
			context.Players.Add(player);
			context.PlayerStats.Add(stat);
			context.SaveChanges();

			return context;
		}

		[Fact]
		public async Task AddStatAsync_AddsStat()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			var model = new CreatePlayerStatViewModel
			{
				PlayerId = 1,
				ClubId = 1,
				Season = "2024/25",
				Appearances = 28,
				Goals = 20,
				Assists = 10
			};

			await service.AddStatAsync(model);

			var stats = await context.PlayerStats.ToListAsync();
			Assert.Equal(2, stats.Count);
			Assert.Contains(stats, s => s.Season == "2024/25" && s.Goals == 20);
		}

		[Fact]
		public async Task UpdateStatAsync_UpdatesStat()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			var model = new EditPlayerStatViewModel
			{
				StatId = 1,
				PlayerId = 1,
				ClubId = 1,
				Season = "2023/24",
				Appearances = 35,
				Goals = 30,
				Assists = 8
			};

			await service.UpdateStatAsync(model);

			var updated = await context.PlayerStats.FindAsync(1);
			Assert.Equal(35, updated.Appearances);
			Assert.Equal(30, updated.Goals);
			Assert.Equal(8, updated.Assists);
		}

		[Fact]
		public async Task DeleteStatAsync_DeletesStat()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			await service.DeleteStatAsync(1);

			var stat = await context.PlayerStats.FindAsync(1);
			Assert.Null(stat);
		}

		[Fact]
		public async Task DeleteStatAsync_InvalidId_DoesNothing()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			await service.DeleteStatAsync(999);

			var count = await context.PlayerStats.CountAsync();
			Assert.Equal(1, count);
		}

		[Fact]
		public async Task GetStatByIdAsync_ValidId_ReturnsModel()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			var model = await service.GetStatByIdAsync(1);

			Assert.NotNull(model);
			Assert.Equal("2023/24", model.Season);
			Assert.NotEmpty(model.Clubs);
		}

		[Fact]
		public async Task GetStatByIdAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			var model = await service.GetStatByIdAsync(999);

			Assert.Null(model);
		}

		[Fact]
		public async Task GetClubsAsync_ReturnsClubs()
		{
			var context = GetDbContext();
			var service = new PlayerStatService(context);

			var clubs = await service.GetClubsAsync();

			Assert.Single(clubs);
			Assert.Equal("Juventus", clubs.First().Name);
		}
	}
}
