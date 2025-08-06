using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core;
using TransfermarketApp.ViewModels.Clubs;
using Xunit;
using System.Collections.Generic;
using TransfermarketApp.Data.Models.Enums;

namespace TransfermarketApp.Tests
{
	public class ClubServiceTests
	{
		private TransfermarketAppDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<TransfermarketAppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new TransfermarketAppDbContext(options);

			var premierLeague = new League
			{
				LeagueId = 1,
				Name = "Premier League",
				Country = "England",
				Level = "First Division"
			};

			var laLiga = new League
			{
				LeagueId = 2,
				Name = "La Liga",
				Country = "Spain",
				Level = "First Division"
			};

			context.Leagues.AddRange(premierLeague, laLiga);

			var arsenal = new Club
			{
				ClubId = 1,
				Name = "Arsenal",
				FoundedYear = 1886,
				Budget = 100_000_000,
				ImageUrl = "arsenal.png",
				LeagueId = premierLeague.LeagueId,
				League = premierLeague,
				Players = new List<Player>
		{
			new Player
			{
				PlayerId = 1,
				Name = "Bukayo Saka",
				Age = 22,
				MarketValue = 90000000,
				ImageUrl = "saka.png",
				Position = Position.RW
			}
		}
			};

			var realMadrid = new Club
			{
				ClubId = 2,
				Name = "Real Madrid",
				FoundedYear = 1902,
				Budget = 700000000,
				ImageUrl = "realmadrid.png",
				LeagueId = laLiga.LeagueId,
				League = laLiga
			};

			var manchesterUnited = new Club
			{
				ClubId = 3,
				Name = "Manchester United",
				FoundedYear = 1878,
				Budget = 650000000,
				ImageUrl = "manutd.png",
				LeagueId = premierLeague.LeagueId,
				League = premierLeague
			};

			context.Clubs.AddRange(arsenal, realMadrid, manchesterUnited);
			context.SaveChanges();

			return context;
		}



		[Fact]
		public async Task GetAllClubsAsync_ReturnsAllClubs()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetAllClubsAsync();

			Assert.Equal(3, result.Count());
			Assert.Contains(result, c => c.Name == "Arsenal");
		}

		[Fact]
		public async Task GetClubByIdAsync_ValidId_ReturnsClubDetails()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetClubByIdAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Arsenal", result.Name);
			Assert.Single(result.Players);
		}

		[Fact]
		public async Task GetClubByIdAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetClubByIdAsync(999);

			Assert.Null(result);
		}

		[Fact]
		public async Task CreateClubAsync_AddsNewClub()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var model = new CreateClubViewModel
			{
				Name = "Chelsea",
				FoundedYear = 1905,
				Budget = 90000000,
				ImageUrl = "chelsea.png",
				LeagueId = 1
			};

			await service.CreateClubAsync(model);

			var clubs = await context.Clubs.ToListAsync();
			Assert.Equal(4, clubs.Count);
			Assert.Contains(clubs, c => c.Name == "Chelsea");
		}

		[Fact]
		public async Task GetClubForEditAsync_ValidId_ReturnsEditModel()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetClubForEditAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Arsenal", result.Name);
		}

		[Fact]
		public async Task GetClubForEditAsync_InvalidId_ReturnsNull()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetClubForEditAsync(999);

			Assert.Null(result);
		}

		[Fact]
		public async Task UpdateClubAsync_ValidId_UpdatesClub()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var model = new EditClubViewModel
			{
				ClubId = 1,
				Name = "Arsenal FC",
				FoundedYear = 1886,
				Budget = 120000000,
				ImageUrl = "new.png",
				LeagueId = 1
			};

			await service.UpdateClubAsync(1, model);

			var updated = await context.Clubs.FindAsync(1);
			Assert.Equal("Arsenal FC", updated.Name);
			Assert.Equal(120000000, updated.Budget);
		}

		[Fact]
		public async Task UpdateClubAsync_InvalidId_ThrowsException()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var model = new EditClubViewModel
			{
				ClubId = 999,
				Name = "Non-existent",
				FoundedYear = 2000,
				Budget = 5000000,
				ImageUrl = "none.png",
				LeagueId = 1
			};

			await Assert.ThrowsAsync<Exception>(() => service.UpdateClubAsync(999, model));
		}

		[Fact]
		public async Task DeleteClubAsync_RemovesClub()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			await service.DeleteClubAsync(1);

			var club = await context.Clubs.FindAsync(1);
			Assert.Null(club);
		}

		[Fact]
		public async Task DeleteClubAsync_InvalidId_DoesNothing()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			await service.DeleteClubAsync(999);

			var count = await context.Clubs.CountAsync();
			Assert.Equal(3, count);
		}

		[Fact]
		public async Task GetLeaguesAsync_ReturnsAllLeagues()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var result = await service.GetLeaguesAsync();

			Assert.Equal(2, result.Count());
			Assert.Contains(result, l => l.Name == "Premier League");
			Assert.Contains(result, l => l.Name == "La Liga");
		}
		[Fact]
		public async Task GetFilteredClubsAsync_ReturnsFilteredResults()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var clubs = await service.GetFilteredClubsAsync(searchTerm: "Real", leagueId: 2, page: 1, pageSize: 10);

			var clubList = clubs.ToList();

			Assert.Single(clubList);
			Assert.Equal("Real Madrid", clubList[0].Name);
			Assert.Equal(2, clubList[0].LeagueId);
		}

		[Fact]
		public async Task GetFilteredClubsCountAsync_WithFilters_ReturnsCorrectCount()
		{
			var context = GetDbContext();
			var service = new ClubService(context);

			var count = await service.GetFilteredClubsCountAsync(searchTerm: "Manchester", leagueId: 1);

			Assert.Equal(1, count);
		}
	}
}
