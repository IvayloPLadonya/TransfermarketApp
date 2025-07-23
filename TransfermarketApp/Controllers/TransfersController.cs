using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransfermarketApp.Services.Core.Contracts;
using TransfermarketApp.ViewModels.Transfers;

namespace TransfermarketApp.Controllers
{
	public class TransfersController : Controller
	{
		private readonly ITransferService _transferService;

		public TransfersController(ITransferService transferService)
		{
			_transferService = transferService;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index(string? playerName, string? clubName, DateTime? fromDate, DateTime? toDate)
		{
			var filter = new TransferFilterViewModel
			{
				PlayerName = playerName,
				ClubName = clubName,
				FromDate = fromDate,
				ToDate = toDate
			};

			var transfers = await _transferService.GetAllTransfersAsync(filter);

			ViewBag.Filter = filter;
			return View(transfers);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var model = await _transferService.GetCreateModelAsync();
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateTransferViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Clubs = await _transferService.GetCreateModelAsync().ContinueWith(t => t.Result.Clubs);
				model.Players = await _transferService.GetCreateModelAsync().ContinueWith(t => t.Result.Players);
				return View(model);
			}

			await _transferService.CreateTransferAsync(model);
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _transferService.GetTransferForEditAsync(id);
			if (model == null) return NotFound();
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditTransferViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Clubs = await _transferService.GetCreateModelAsync().ContinueWith(t => t.Result.Clubs);
				model.Players = await _transferService.GetCreateModelAsync().ContinueWith(t => t.Result.Players);
				return View(model);
			}

			await _transferService.UpdateTransferAsync(model);
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _transferService.GetTransferForEditAsync(id);
			if (model == null) return NotFound();
			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _transferService.DeleteTransferAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
