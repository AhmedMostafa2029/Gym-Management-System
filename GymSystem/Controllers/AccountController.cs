using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AccountViewModels;
using GymSystem.Controllers;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAnalyticsServices analyticsServices;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAnalyticsServices analyticsServices)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.analyticsServices = analyticsServices;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var analytics = await analyticsServices.GetAnalyticsDataAsync();

            var viewModel = new LoginPageViewModel
            {
                Analytics = analytics
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Analytics =
                    await analyticsServices.GetAnalyticsDataAsync();

                return View(model);
            }

            var user =
                await userManager.FindByEmailAsync(model.Login.Email);

            if (user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid Email Or Password.");

                model.Analytics =
                    await analyticsServices.GetAnalyticsDataAsync();

                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(
                user.UserName,
                model.Login.Password,
                model.Login.RememberMe,
                true);

            if (result.Succeeded)
            {
                return RedirectToAction(
                    nameof(HomeController.Index),
                    "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This Account is Locked Out.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Account is Unauthorized.");
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid Email Or Password.");
            }

            model.Analytics =
                await analyticsServices.GetAnalyticsDataAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}