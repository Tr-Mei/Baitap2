using Baitap2.Data;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Baitap2.Controllers
{
    public class AuthController : Controller
    {
        private readonly DemoContext _context;

        public AuthController(DemoContext context)
        {
            _context = context;
        }

        // ===== LOGIN =====
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var user = _context.NguoiDungs
                .FirstOrDefault(x => x.Username == username && x.MatKhau == password);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            if (user.BiKhoa)
            {
                ViewBag.Error = "Tài khoản đã bị khóa";
                return View();
            }

            if (!user.IsActive)
            {
                ViewBag.Error = "Tài khoản chưa kích hoạt";
                return View();
            }

            HttpContext.Session.Clear();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.VaiTro.ToString());

            if (user.VaiTro == VaiTro.Admin)
                return RedirectToAction("Dashboard", "Admin");

            if (user.VaiTro == VaiTro.TaiXe)
                return RedirectToAction("Index", "TaiXe");

            return RedirectToAction("DatChuyen", "Khach");
        }

        // ===== LOGOUT =====
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ===== REGISTER =====
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Register(NguoiDung model)
        //{
        //    // 1. Validate
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // 2. Check trùng
        //    if (_context.NguoiDungs.Any(x => x.Username == model.Username))
        //    {
        //        ModelState.AddModelError("Username", "Username đã tồn tại");
        //        return View(model);
        //    }

        //    if (_context.NguoiDungs.Any(x => x.Email == model.Email))
        //    {
        //        ModelState.AddModelError("Email", "Email đã tồn tại");
        //        return View(model);
        //    }

        //    // 3. Save
        //    _context.NguoiDungs.Add(model);
        //    _context.SaveChanges();

        //    return RedirectToAction("Login");
        //}
        [HttpPost]
        public IActionResult Register(NguoiDung model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.NguoiDungs.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username đã tồn tại");
                return View(model);
            }

            if (_context.NguoiDungs.Any(x => x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(model);
            }

            // 🔥 FIX: không cho tự chọn tài xế
            model.VaiTro = VaiTro.Khach;
            model.IsActive = true;
            model.BiKhoa = false;

            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}
