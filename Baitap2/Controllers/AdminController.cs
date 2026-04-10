using Microsoft.AspNetCore.Mvc;
using Baitap2.Data;
using Baitap2.Models;
using System.Linq;

public class AdminController : Controller
{
    private readonly DemoContext _context;

    public AdminController(DemoContext context)
    {
        _context = context;
    }

    // ===== CHECK ADMIN =====
    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("Role") == "Admin";
    }

    // ===== DASHBOARD =====
    public IActionResult Dashboard()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");
        return View();
    }

    // =========================
    // 👤 KHÁCH HÀNG
    // =========================
    public IActionResult KhachHang(int page = 1)
    {
        int pageSize = 5;

        var query = _context.NguoiDungs
            .Where(x => x.VaiTro == VaiTro.Khach);

        var total = query.Count();

        var data = query
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

        return PartialView("KhachHang", data);
    }

    // =========================
    // 🚗 TÀI XẾ
    // =========================
    public IActionResult TaiXe(int page = 1)
    {
        int pageSize = 5;

        var query = _context.NguoiDungs
            .Where(x => x.VaiTro == VaiTro.TaiXe);

        var total = query.Count();

        var data = query
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

        return PartialView("TaiXe", data);
    }
    // =========================
    // 🔍 CHI TIẾT USER
    // =========================
    public IActionResult ChiTiet(int id)
    {
        var user = _context.NguoiDungs.Find(id);
        return PartialView("ChiTiet", user);
    }

    // =========================
    // ❌ XÓA
    // =========================
    public IActionResult Xoa(int id)
    {
        var user = _context.NguoiDungs.Find(id);

        if (user != null)
        {
            var role = user.VaiTro;

            _context.NguoiDungs.Remove(user);
            _context.SaveChanges();

            if (role == VaiTro.TaiXe)
                return RedirectToAction("TaiXe");

            return RedirectToAction("KhachHang");
        }

        return RedirectToAction("Dashboard");
    }

    // =========================
    // 🔒 KHÓA
    // =========================
    public IActionResult Khoa(int id)
    {
        var user = _context.NguoiDungs.Find(id);

        if (user != null)
        {
            user.IsActive = false;
            _context.SaveChanges();

            if (user.VaiTro == VaiTro.TaiXe)
                return RedirectToAction("TaiXe");

            return RedirectToAction("KhachHang");
        }

        return RedirectToAction("Dashboard");
    }

    // =========================
    // 🔓 MỞ KHÓA
    // =========================
    public IActionResult MoKhoa(int id)
    {
        var user = _context.NguoiDungs.Find(id);

        if (user != null)
        {
            user.IsActive = true;
            _context.SaveChanges();

            if (user.VaiTro == VaiTro.TaiXe)
                return RedirectToAction("TaiXe");

            return RedirectToAction("KhachHang");
        }

        return RedirectToAction("Dashboard");
    }

    // =========================
    // ✏️ SỬA
    // =========================
    public IActionResult Sua(int id)
    {
        var user = _context.NguoiDungs.Find(id);
        return PartialView("Sua", user);
    }

    [HttpPost]
    public IActionResult Sua(NguoiDung u)
    {
        _context.NguoiDungs.Update(u);
        _context.SaveChanges();

        if (u.VaiTro == VaiTro.TaiXe)
            return RedirectToAction("TaiXe");

        return RedirectToAction("KhachHang");
    }

    // =========================
    // 🚕 CHUYẾN ĐI
    // =========================
    //public IActionResult ChuyenDis()
    //{
    //    var list = _context.ChuyenDis.ToList();
    //    return PartialView("ChuyenDis", list);
    //}

    public IActionResult ChuyenDis(int page = 1)
    {
        int pageSize = 5;

        var total = _context.ChuyenDis.Count();

        var data = _context.ChuyenDis
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

        return PartialView("ChuyenDis", data); // 🔥 phải là PartialView
    }


    // =========================
    // 💰 QUẢN LÝ GIÁ
    // =========================
    public IActionResult Gia(int page = 1)
    {
        int pageSize = 5;

        var total = _context.ChuyenDis.Count();

        var data = _context.ChuyenDis
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

        return PartialView("Gia", data);
    }

    [HttpPost]
    public IActionResult CapNhatGia(int id, decimal gia)
    {
        var c = _context.ChuyenDis.Find(id);

        if (c != null)
        {
            c.GiaDuKien = gia;
            _context.SaveChanges();
        }

        return RedirectToAction("Gia");
    }

    // =========================
    // 📊 BÁO CÁO
    // =========================
    public IActionResult BaoCao()
    {
        var tongTien = _context.ThanhToans
            .Where(x => x.TrangThai == "DaTT")
            .Sum(x => (decimal?)x.SoTien) ?? 0;

        var soChuyen = _context.ChuyenDis.Count();
        var soUser = _context.NguoiDungs.Count();

        ViewBag.TongTien = tongTien;
        ViewBag.SoChuyen = soChuyen;
        ViewBag.SoUser = soUser;

        return PartialView("BaoCao");
    }
}
