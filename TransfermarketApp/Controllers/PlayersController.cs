using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransfermarketApp.Data;
using TransfermarketApp.Data.Models.Enums;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.Players;

namespace TransfermarketApp.Controllers
{
	public class PlayersController : Controller
	{
		private readonly IPlayerService _playerService;
		public PlayersController(IPlayerService playerService)
		{
			_playerService = playerService;
		}

		public async Task<IActionResult> Index(PlayerFilterViewModel filter, int page = 1)
		{
			const int pageSize = 10;

			var players = await _playerService.GetFilteredPlayersAsync(filter, page, pageSize);
			var totalPlayers = await _playerService.GetFilteredPlayersCountAsync(filter);

			ViewBag.TotalPages = (int)Math.Ceiling(totalPlayers / (double)pageSize);
			ViewBag.CurrentPage = page;
			ViewBag.Filter = filter;

			filter.Clubs = await _playerService.GetClubsAsync();

			return View(players);
		}




		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var player = await _playerService.GetPlayerByIdAsync(id);
			if (player == null) return NotFound();
			return View(player);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var model = new CreatePlayerViewModel
			{
				Clubs = await _playerService.GetClubsAsync()
			};
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreatePlayerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Clubs = await _playerService.GetClubsAsync();
				return View(model);
			}

			await _playerService.CreatePlayerAsync(model);
			return RedirectToAction(nameof(Index));
		}



		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _playerService.GetPlayerForEditAsync(id);
			if (model == null) return NotFound();

			model.Clubs = await _playerService.GetClubsAsync();
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditPlayerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Clubs = await _playerService.GetClubsAsync();
				return View(model);
			}

			await _playerService.UpdatePlayerAsync(id, model);
			return RedirectToAction(nameof(Index));
		}



		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var player = await _playerService.GetPlayerByIdAsync(id);
			if (player == null) return NotFound();
			return View(player);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _playerService.DeletePlayerAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}

