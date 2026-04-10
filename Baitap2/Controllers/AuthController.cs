



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
            var user = _context.NguoiDungs
                .FirstOrDefault(x => x.Username == username && x.MatKhau == password);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            // 🔥 QUAN TRỌNG: clear session cũ
            HttpContext.Session.Clear();

            // 🔥 FIX CHUẨN: dùng INT
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.VaiTro.ToString());

            // 👉 Redirect theo role
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

        [HttpPost]
        public IActionResult Register(string username, string email, string password, string vaiTro, string dienThoai)
        {
            var role = Enum.Parse<VaiTro>(vaiTro);

            var user = new NguoiDung
            {
                Username = username,
                Email = email,
                MatKhau = password,
                VaiTro = role,
                DienThoai = dienThoai
            };

            _context.NguoiDungs.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
