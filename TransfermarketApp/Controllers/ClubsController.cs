using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransfermarketApp.Data.Models;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.Clubs;

namespace TransfermarketApp.Controllers
{
	public class ClubsController : Controller
	{
		private readonly IClubService _clubService;

		public ClubsController(IClubService clubService)
		{
			_clubService = clubService;
		}

		public async Task<IActionResult> Index(string? searchTerm, int? leagueId, int page = 1)
		{
			const int pageSize = 10;

			var clubs = await _clubService.GetFilteredClubsAsync(searchTerm, leagueId, page, pageSize);
			var totalCount = await _clubService.GetFilteredClubsCountAsync(searchTerm, leagueId);

			var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewBag.SearchTerm = searchTerm;
			ViewBag.LeagueId = leagueId;

			var leagues = await _clubService.GetLeaguesAsync();
			ViewBag.Leagues = leagues;

			return View(clubs);
		}


		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var club = await _clubService.GetClubByIdAsync(id);
			if (club == null) return NotFound();
			return View(club);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var leagues = await _clubService.GetLeaguesAsync();

			var viewModel = new CreateClubViewModel
			{
				Leagues = leagues
			};

			return View(viewModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateClubViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Leagues = await _clubService.GetLeaguesAsync();
				return View(model);
			}

			await _clubService.CreateClubAsync(model);
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _clubService.GetClubForEditAsync(id);
			if (model == null) return NotFound();

			model.Leagues = await _clubService.GetLeaguesAsync();

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditClubViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			await _clubService.UpdateClubAsync(id, model);
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var club = await _clubService.GetClubByIdAsync(id);
			if (club == null) return NotFound();
			return View(club);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _clubService.DeleteClubAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
