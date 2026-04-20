
using Microsoft.AspNetCore.Mvc;
using Baitap2.Data;
using Baitap2.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        int pageSize = 3;

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
        int pageSize = 3;

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
    // ➕ TẠO TÀI XẾ
    // =========================
    public IActionResult TaoTaiXe()
    {
        return View();
    }

    [HttpPost]
    public IActionResult TaoTaiXe(NguoiDung user, TaiXe tx)
    {
        // 🔥 validate
        if (string.IsNullOrEmpty(tx.CCCD))
        {
            return Content("❌ Thiếu CCCD");
        }

        // 🔥 set user
        user.VaiTro = VaiTro.TaiXe;
        user.IsActive = true;
        user.BiKhoa = false;

        _context.NguoiDungs.Add(user);
        _context.SaveChanges();

        // 🔥 gán FK
        tx.NguoiDungId = user.Id;

        // 🔥 lấy SĐT từ user (tránh trùng dữ liệu)
        tx.SoDienThoai = user.DienThoai;

        tx.Online = false;
        tx.DanhGiaTB = 5;

        _context.TaiXes.Add(tx);
        _context.SaveChanges();

        return RedirectToAction("TaiXe");
    }

    // =========================
    // 🔍 CHI TIẾT
    // =========================
    public IActionResult ChiTiet(int id)
    {
        var user = _context.NguoiDungs.FirstOrDefault(x => x.Id == id);

        TaiXe tx = null;

        if (user != null && user.VaiTro == VaiTro.TaiXe)
        {
            tx = _context.TaiXes
                .FirstOrDefault(x => x.NguoiDungId == user.Id);
        }

        var model = new Tuple<NguoiDung, TaiXe>(user, tx);

        return PartialView("ChiTiet", model);
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
    // 🔒 KHÓA / MỞ
    // =========================
    public IActionResult Khoa(int id)
    {
        var user = _context.NguoiDungs.Find(id);

        if (user != null)
        {
            user.IsActive = false;
            _context.SaveChanges();
        }

        return RedirectToAction(user?.VaiTro == VaiTro.TaiXe ? "TaiXe" : "KhachHang");
    }

    public IActionResult MoKhoa(int id)
    {
        var user = _context.NguoiDungs.Find(id);

        if (user != null)
        {
            user.IsActive = true;
            _context.SaveChanges();
        }

        return RedirectToAction(user?.VaiTro == VaiTro.TaiXe ? "TaiXe" : "KhachHang");
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

        return RedirectToAction(u.VaiTro == VaiTro.TaiXe ? "TaiXe" : "KhachHang");
    }

    // =========================
    // 🚕 CHUYẾN ĐI
    // =========================
    public IActionResult ChuyenDis(int page = 1)
    {
        int pageSize = 3;

        var total = _context.ChuyenDis.Count();

        var data = _context.ChuyenDis
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

        return PartialView("ChuyenDis", data);
    }

    public IActionResult ChiTietChuyenDi(int id)
    {
        var chuyen = _context.ChuyenDis
            .Include(x => x.Khach)
            .Include(x => x.TaiXe)
            .ThenInclude(tx => tx.NguoiDung)
            .FirstOrDefault(x => x.Id == id);

        if (chuyen == null)
            return NotFound();

        return View(chuyen);
    }

    // =========================
    // 💰 GIÁ
    // =========================
    public IActionResult Gia(int page = 1)
    {
        int pageSize = 3;

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
        var tongTien = _context.ChuyenDis
            .Where(x => x.TrangThai == TrangThai.HoanThanh)
            .Sum(x => (decimal?)x.GiaDuKien) ?? 0;

        var soChuyen = _context.ChuyenDis
            .Count(x => x.TrangThai == TrangThai.HoanThanh);

        var soUser = _context.NguoiDungs
            .Count(x => x.IsActive);

        ViewBag.TongTien = tongTien;
        ViewBag.SoChuyen = soChuyen;
        ViewBag.SoUser = soUser;

        return PartialView("BaoCao");
    }
}
