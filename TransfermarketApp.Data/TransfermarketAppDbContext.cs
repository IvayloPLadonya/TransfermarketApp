using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TransfermarketApp.Data.Models;

namespace TransfermarketApp.Data
{
	public class TransfermarketAppDbContext : IdentityDbContext
	{
		public TransfermarketAppDbContext(DbContextOptions<TransfermarketAppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Player> Players { get; set; } = null!;
		public DbSet<Club> Clubs { get; set; } = null!;
		public DbSet<League> Leagues { get; set; } = null!;
		public DbSet<Transfer> Transfers { get; set; } = null!;
		public DbSet<PlayerStat> PlayerStats { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Player>()
				.HasOne(p => p.CurrentClub)
				.WithMany(c => c.Players)
				.HasForeignKey(p => p.CurrentClubId)
				.OnDelete(DeleteBehavior.SetNull);


			modelBuilder.Entity<Club>()
				.HasOne(c => c.League)
				.WithMany(l => l.Clubs)
				.HasForeignKey(c => c.LeagueId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Transfer>()
				.HasOne(t => t.Player)
				.WithMany(p => p.Transfers)
				.HasForeignKey(t => t.PlayerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Transfer>()
				.HasOne(t => t.FromClub)
				.WithMany(c => c.OutgoingTransfers)
				.HasForeignKey(t => t.FromClubId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Transfer>()
				.HasOne(t => t.ToClub)
				.WithMany(c => c.IncomingTransfers)
				.HasForeignKey(t => t.ToClubId)
				.OnDelete(DeleteBehavior.Restrict);

			
			modelBuilder.Entity<PlayerStat>()
				.HasOne(ps => ps.Player)
				.WithMany(p => p.PlayerStats)
				.HasForeignKey(ps => ps.PlayerId)
				.OnDelete(DeleteBehavior.Cascade);
			
			modelBuilder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.LoginProvider)
				.HasMaxLength(450);

			modelBuilder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.ProviderKey)
				.HasMaxLength(450);

			modelBuilder.Entity<IdentityUserToken<string>>()
				.Property(t => t.LoginProvider)
				.HasMaxLength(450);

			modelBuilder.Entity<IdentityUserToken<string>>()
				.Property(t => t.Name)
				.HasMaxLength(450);
		}
	}
}
