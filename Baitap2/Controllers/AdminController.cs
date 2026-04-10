
using Baitap2.Data;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AdminController : Controller
{
    private readonly DemoContext _context;

    public AdminController(DemoContext context)
    {
        _context = context;
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("Role") == "Admin";
    }

    // ===== DASHBOARD =====
    public IActionResult Dashboard()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");

        ViewBag.TongNguoiDung = _context.NguoiDungs.Count();
        ViewBag.TongTaiXe = _context.TaiXes.Count();
        ViewBag.TongChuyen = _context.ChuyenDis.Count();

        ViewBag.DoanhThu = _context.ThanhToans
            .Where(x => x.TrangThai == "DaTT")
            .Sum(x => (decimal?)x.SoTien) ?? 0;

        return View();
    }

    // ===== QUẢN LÝ USER =====
    public IActionResult Users()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");

        return View(_context.NguoiDungs.ToList());
    }

    public IActionResult ToggleUser(int id)
    {
        var u = _context.NguoiDungs.Find(id);
        if (u != null)
        {
            u.IsActive = !u.IsActive;
            _context.SaveChanges();
        }
        return RedirectToAction("Users");
    }

    // ===== QUẢN LÝ TÀI XẾ =====
    public IActionResult TaiXes()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");

        return View(_context.TaiXes.ToList());
    }

    public IActionResult ToggleOnline(int id)
    {
        var tx = _context.TaiXes.Find(id);
        if (tx != null)
        {
            tx.Online = !tx.Online;
            _context.SaveChanges();
        }
        return RedirectToAction("TaiXes");
    }

    // ===== QUẢN LÝ CHUYẾN =====
    public IActionResult ChuyenDis()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");

        return View(_context.ChuyenDis.ToList());
    }

    // ===== ĐIỀU PHỐI =====
    public IActionResult DieuPhoi(int id)
    {
        var chuyen = _context.ChuyenDis.Find(id);
        var taiXes = _context.TaiXes.Where(x => x.Online).ToList();

        ViewBag.TaiXes = taiXes;
        return View(chuyen);
    }

    [HttpPost]
    public IActionResult DieuPhoi(int chuyenId, int taiXeId)
    {
        var c = _context.ChuyenDis.Find(chuyenId);

        if (c != null)
        {
            c.TaiXeId = taiXeId;
            c.TrangThai = TrangThai.DaNhan;
            _context.SaveChanges();
        }

        return RedirectToAction("ChuyenDis");
    }

    // ===== BÁO CÁO =====
    public IActionResult BaoCao()
    {
        var tongTien = _context.ThanhToans
            .Where(x => x.TrangThai == "DaTT")
            .Sum(x => (decimal?)x.SoTien) ?? 0;

        var soChuyen = _context.ChuyenDis.Count();

        ViewBag.TongTien = tongTien;
        ViewBag.SoChuyen = soChuyen;

        return View();
    }
}
