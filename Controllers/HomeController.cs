using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QassimLabBooking.Data;
using QassimLabBooking.Models;

namespace QassimLabBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var labs = _context.Labs.Include(l => l.Devices).ToList();
            return View(labs);
        }

        [HttpPost]
        public IActionResult BookLab(Reservation model)
        {
            // استبعاد التحقق الإجباري عن كائنات التنقل
            ModelState.Remove("Lab");
            ModelState.Remove("Device");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                // فحص التداخل الزمني الحقيقي لنفس الجهاز في نفس اليوم:
                // الشرط الرياضي للتداخل: (بداية الجديد < نهاية القديم) AND (نهابة الجديد > بداية القديم)
                bool isConflict = _context.Reservations.Any(r =>
                    r.DeviceId == model.DeviceId &&
                    r.BookingDate == model.BookingDate &&
                    string.Compare(model.StartTime, r.EndTime) < 0 &&
                    string.Compare(model.EndTime, r.StartTime) > 0
                );

                if (isConflict)
                {
                    TempData["Error"] = "عذراً، يوجد تعارض في الموعد! هذا الجهاز محجوز مسبقاً في فترة تتداخل مع هذا الوقت.";
                    var labsData = _context.Labs.Include(l => l.Devices).ToList();
                    return View("Index", labsData);
                }

                try
                {
                    model.Status = "مقبول";
                    _context.Reservations.Add(model);
                    _context.SaveChanges();
                    TempData["Success"] = "تم تسجيل الحجز بنجاح!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "حدث خطأ أثناء الحفظ: " + ex.Message;
                    var labsList = _context.Labs.Include(l => l.Devices).ToList();
                    return View("Index", labsList);
                }
            }

            var errors = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            TempData["Error"] = "فشل التحقق من البيانات: " + errors;

            var labs = _context.Labs.Include(l => l.Devices).ToList();
            return View("Index", labs);
        }

        public IActionResult PublicReservations()
        {
            var reservations = _context.Reservations
                .Include(r => r.Lab)
                .Include(r => r.Device)
                .ToList();
            return View(reservations);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}