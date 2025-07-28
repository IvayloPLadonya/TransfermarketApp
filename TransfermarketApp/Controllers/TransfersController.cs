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

		public async Task<IActionResult> Index(TransferFilterViewModel filter, int page = 1)
		{
			const int pageSize = 10;

			var transfers = await _transferService.GetFilteredTransfersAsync(filter, page, pageSize);
			var totalCount = await _transferService.GetFilteredTransfersCountAsync(filter);

			ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			ViewBag.CurrentPage = page;
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
