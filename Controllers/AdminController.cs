using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QassimLabBooking.Data;
using QassimLabBooking.Models;

namespace QassimLabBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // بيانات الدخول الافتراضية
            if (username == "Shahd" && password == "Shadel2026$")
            {
                HttpContext.Session.SetString("IsAdmin", "True");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "اسم المستخدم أو كلمة المرور غير صحيحة";
            return View();
        }

        // لوحة التحكم لعرض الحجوزات وإدارتها
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return RedirectToAction("Login");
            }

            var reservations = _context.Reservations
                .Include(r => r.Lab)
                .Include(r => r.Device)
                .ToList();

            return View(reservations);
        }

        // حذف حجز
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return RedirectToAction("Login");

            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                TempData["Success"] = "تم حذف الحجز بنجاح!";
            }
            return RedirectToAction("Dashboard");
        }

        // تسجيل الخروج
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}