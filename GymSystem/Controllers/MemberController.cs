using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    // ViewBag is used to pass data from controller to view without using a strongly typed model. It is a dynamic object that allows you to store and retrieve data using properties.
    // ViewData is a dictionary that allows you to pass data from controller to view. It is similar to ViewBag but it uses string keys to store and retrieve data.
    // TempData is a dictionary that allows you to pass data from one request to another. It is used to store data that needs to be persisted across multiple requests, such as error messages or success messages.
    // Action => Action by TempData

    [Authorize(Roles ="SuperAdmin")] // anly uesd by superAdmin

    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        public async Task<IActionResult> Index(MemberFilterViewModel filter, CancellationToken ct)
        {
            var members = await _memberServices.GetAllAsync(filter,ct);

            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _memberServices.CreateAsync(model, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetByIdAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }


        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var record = await _memberServices.GetHealthRecordAsync(id, ct);

            if (record is null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(record);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetForUpdateAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            MemberToUpdateViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _memberServices.UpdateAsync(id, model, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetByIdAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberServices.DeleteAsync(id, ct);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Member deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
