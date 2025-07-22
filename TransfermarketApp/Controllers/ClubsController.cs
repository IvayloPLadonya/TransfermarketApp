using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		public async Task<IActionResult> Index(string searchTerm, int page = 1)
		{
			const int pageSize = 10;

			var allClubs = await _clubService.GetAllClubsAsync();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				allClubs = allClubs
					.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
					.ToList();
			}

			var totalItems = allClubs.Count();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			var paginatedClubs = allClubs
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewBag.SearchTerm = searchTerm;

			return View(paginatedClubs);
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
