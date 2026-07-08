using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {

        private readonly IAnalyticsServices analyticsServices;

        public HomeController(IAnalyticsServices analyticsServices)
        {
            this.analyticsServices = analyticsServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await analyticsServices.GetAnalyticsDataAsync(ct);
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
