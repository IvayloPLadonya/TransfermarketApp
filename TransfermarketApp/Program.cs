using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TransfermarketApp.Data;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.Services.Core;

namespace TransfermarketApp
{
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<TransfermarketAppDbContext>(options =>
	            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddDefaultIdentity<IdentityUser>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 0;
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<TransfermarketAppDbContext>();
			// Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
			builder.Services.AddScoped<IPlayerService, PlayerService>();
			builder.Services.AddScoped<IClubService, ClubService>();
			builder.Services.AddScoped<IPlayerStatService, PlayerStatService>();
			builder.Services.AddScoped<ITransferService, TransferService>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            using (var scope = app.Services.CreateScope())
            {
                 var services = scope.ServiceProvider;
				await DataSeeder.SeedRolesAndAdminAsync(services);
            }

            app.Run();
        }
    }
}
