using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    [Authorize]

    public class SessionController : Controller
    {

        private readonly ISessionServices _sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await _sessionServices.GetAllSessionsAsync(ct);
            return View(Sessions);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }

            var Result = await _sessionServices.CreateSessionAsync(model, ct);

            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = Result.Error;
            await PopulateDropDownAsync(ct);
            return View(model);

        }

        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            var trainers = await _sessionServices.GetTrainersForDropDownAsync(ct);
            var categories = await _sessionServices.GetCategoriesForDropDownAsync(ct);

            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await _sessionServices.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await _sessionServices.GetSessionByIdForUpdateAsync(id, ct);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Edit, it's Not Found";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownAsync(ct);
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }

            var Result = await _sessionServices.UpdateSessionAsync(id, model, ct);

            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = Result.Error;
            await PopulateDropDownAsync(ct);
            return View(model);

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await _sessionServices.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(session);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _sessionServices.DeleteSessionAsync(id, ct);
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
