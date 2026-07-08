using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public async Task<IActionResult> Index()
          => View(await _bookingService.GetAllSessionsAsync());


        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
          => View(await _bookingService.GetMembersForUpcomingBySessionIdAsync(id, ct));

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
            => View(await _bookingService.GetMembersForOngoingBySessionIdAsync(id, ct));

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMembersForDropDownAsync(id, ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            var result = await _bookingService.CreateNewBookingAsync(model, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Booking created successfully." : result.Error;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingService.CancelBookingAsync(memberId, sessionId, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Booking cancelled successfully." : result.Error;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await _bookingService.MarkAttendedAsync(memberId, sessionId, ct);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Attendance recorded." : result.Error;
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }
    }
}
