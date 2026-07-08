using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {

        private readonly ITrainerServices _trainerServices;

        public TrainerController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerServices.GetAllAsync(ct);

            return View(trainers);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerServices.CreateAsync(model, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetForUpdateAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerServices.UpdateAsync(id, model, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerServices.DeleteAsync(id, ct);

            if (result.IsSuccess)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = result.Error;

            return RedirectToAction(nameof(Index));
        }
    }
}
