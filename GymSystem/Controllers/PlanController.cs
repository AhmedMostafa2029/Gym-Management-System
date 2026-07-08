using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymSystem.Controllers
{
    [Authorize]

    public class PlanController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planServices.GetAllAsync(ct);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var plan = await _planServices.GetByIdAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken ct)
        {
            var plan = await _planServices.GetForUpdateAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,UpdatePlanViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _planServices.UpdateAsync(id, model, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(
            int id,
            CancellationToken ct)
        {
            var result =
                await _planServices.ChangeStatusAsync(id, ct);


            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] =
                    "Plan status changed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] =
                    result.Error;
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
