using Microsoft.AspNetCore.Mvc;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.PlayerStats;

namespace TransfermarketApp.Controllers
{
	public class PlayerStatController : Controller
	{
		private readonly IPlayerStatService _playerStatService;

		public PlayerStatController(IPlayerStatService playerStatService)
		{
			_playerStatService = playerStatService;
		}

		[HttpGet]
		public async Task<IActionResult> Create(int playerId)
		{
			var model = new CreatePlayerStatViewModel
			{
				PlayerId = playerId,
				Clubs = await _playerStatService.GetClubsAsync()
			};

			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreatePlayerStatViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Clubs = await _playerStatService.GetClubsAsync();
				return View(model);
			}

			await _playerStatService.AddStatAsync(model);
			return RedirectToAction("Details", "Players", new { id = model.PlayerId });
		}


		public async Task<IActionResult> Edit(int id)
		{
			var stat = await _playerStatService.GetStatByIdAsync(id);
			if (stat == null)
				return NotFound();

			return View(stat);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditPlayerStatViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			await _playerStatService.UpdateStatAsync(model);
			return RedirectToAction("Details", "Players", new { id = model.PlayerId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id, int playerId)
		{
			await _playerStatService.DeleteStatAsync(id);
			return RedirectToAction("Details", "Players", new { id = playerId });
		}

	}
}

