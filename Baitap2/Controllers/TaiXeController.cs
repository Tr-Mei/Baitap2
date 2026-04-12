


//using Baitap2.Data;
//using Baitap2.Hubs;
//using Baitap2.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;

//public class TaiXeController : Controller
//{
//    private readonly DemoContext _context;
//    private readonly IHubContext<RideHub> _hub;

//    public TaiXeController(DemoContext context, IHubContext<RideHub> hub)
//    {
//        _context = context;
//        _hub = hub;
//    }

//    public IActionResult Index(int? id)
//    {
//        ChuyenDi c = null;

//        if (id != null)
//        {
//            c = _context.ChuyenDis.Find(id);
//        }
//        else
//        {
//            // 🔥 CHỈ LẤY CUỐC ĐANG CHỜ TÀI XẾ
//            c = _context.ChuyenDis
//     .Where(x => x.TrangThai == TrangThai.DangTimTaiXe)
//     .OrderByDescending(x => x.Id)
//     .FirstOrDefault();

//        }

//        return View(c);
//    }


//    private async Task GuiTrangThai(ChuyenDi c)
//    {
//        await _hub.Clients.Group(c.Id.ToString())
//            .SendAsync("NhanCapNhat", new
//            {
//                trangThai = c.TrangThai.ToString()
//            });
//    }

//    public async Task<IActionResult> Nhan(int id)
//    {
//        var c = _context.ChuyenDis.Find(id);
//        if (c == null) return RedirectToAction("Index");

//        c.TrangThai = TrangThai.DaNhan;
//        _context.SaveChanges();

//        await GuiTrangThai(c);
//        return RedirectToAction("Index", new { id });
//    }

//    public async Task<IActionResult> TuChoi(int id)
//    {
//        var c = _context.ChuyenDis.Find(id);
//        if (c == null) return RedirectToAction("Index");

//        // 🔥 Reset lại chuyến (cho tài xế khác nhận)
//        c.TrangThai = TrangThai.DangTimTaiXe;

//        // Nếu có cột TaiXeId thì nhớ reset
//        // c.TaiXeId = null;

//        _context.SaveChanges();

//        // 🔥 Gửi realtime cho khách
//        await GuiTrangThai(c);

//        // 🔥 QUAY VỀ MÀN HÌNH CHÍNH (KHÔNG TRUYỀN ID)
//        return RedirectToAction("Index");
//    }

//    public async Task<IActionResult> Den(int id)
//    {
//        var c = _context.ChuyenDis.Find(id);
//        if (c == null) return RedirectToAction("Index");

//        c.TrangThai = TrangThai.DaDen;
//        _context.SaveChanges();

//        await GuiTrangThai(c);
//        return RedirectToAction("Index", new { id });
//    }

//    public async Task<IActionResult> BatDau(int id)
//    {
//        var c = _context.ChuyenDis.Find(id);
//        if (c == null) return RedirectToAction("Index");

//        c.TrangThai = TrangThai.DangDi;
//        _context.SaveChanges();

//        await GuiTrangThai(c);
//        return RedirectToAction("Index", new { id });
//    }

//    public IActionResult HoanThanh(int id)
//    {
//        var chuyen = _context.ChuyenDis.Find(id);
//        if (chuyen == null) return NotFound();

//        chuyen.TrangThai = TrangThai.HoanThanh;

//        // 🔥 TẠO THANH TOÁN
//        var tt = new ThanhToan
//        {
//            ChuyenDiId = id,
//            SoTien = chuyen.GiaDuKien,
//            TrangThai = "ChuaTT"
//        };

//        _context.ThanhToans.Add(tt);
//        _context.SaveChanges();

//        // 🔥 realtime cho khách
//        _hub.Clients.Group(id.ToString())
//            .SendAsync("NhanCapNhat", new { trangThai = "HoanThanh" });

//        // 👉 CHUYỂN SANG QR
//        return RedirectToAction("QR", "ThanhToan", new { rideId = id });
//    }

//}


using Baitap2.Data;
using Baitap2.Hubs;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class TaiXeController : Controller
{
    private readonly DemoContext _context;
    private readonly IHubContext<RideHub> _hub;

    public TaiXeController(DemoContext context, IHubContext<RideHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    public IActionResult Index(int? id)
    {
        ChuyenDi c = null;

        int? taiXeId = HttpContext.Session.GetInt32("UserId");

        if (id != null)
        {
            c = _context.ChuyenDis.Find(id);
        }
        else
        {
            c = _context.ChuyenDis
                .Where(x => x.TrangThai == TrangThai.DangTimTaiXe
                    && !_context.TuChoiChuyens
                        .Any(t => t.ChuyenDiId == x.Id && t.TaiXeId == taiXeId))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        return View(c);
    }

    private async Task GuiTrangThai(ChuyenDi c)
    {
        await _hub.Clients.Group(c.Id.ToString())
            .SendAsync("NhanCapNhat", new
            {
                trangThai = c.TrangThai.ToString()
            });
    }

    // ===== NHẬN CUỐC (🔥 QUAN TRỌNG NHẤT) =====
    public async Task<IActionResult> Nhan(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return Content("❌ Chưa đăng nhập");

        var user = _context.NguoiDungs.Find(userId);

        if (user == null || user.VaiTro != VaiTro.TaiXe)
            return Content("❌ Bạn không phải tài xế");

        using var tran = _context.Database.BeginTransaction();

        var c = _context.ChuyenDis.FirstOrDefault(x => x.Id == id);

        if (c == null)
        {
            tran.Rollback();
            return RedirectToAction("Index");
        }

        // 🔥 CHỐT: chỉ 1 tài xế nhận được
        if (c.TrangThai != TrangThai.DangTimTaiXe)
        {
            tran.Rollback();
            return Content("❌ Cuốc đã có tài xế khác nhận");
        }

        c.TrangThai = TrangThai.DaNhan;
        c.TaiXeId = userId;

        _context.SaveChanges();
        tran.Commit();

        await GuiTrangThai(c);

        return RedirectToAction("Index", new { id });
    }

    // ===== TỪ CHỐI =====
    public IActionResult TuChoi(int id)
    {
        int? taiXeId = HttpContext.Session.GetInt32("UserId");

        var tc = new TuChoiChuyen
        {
            ChuyenDiId = id,
            TaiXeId = taiXeId ?? 0
        };

        _context.TuChoiChuyens.Add(tc);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // ===== ĐÃ ĐẾN =====
    public async Task<IActionResult> Den(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        var c = _context.ChuyenDis.Find(id);

        if (c == null || c.TaiXeId != userId)
            return Content("❌ Không hợp lệ");

        c.TrangThai = TrangThai.DaDen;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
    }

    // ===== BẮT ĐẦU =====
    public async Task<IActionResult> BatDau(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        var c = _context.ChuyenDis.Find(id);

        if (c == null || c.TaiXeId != userId)
            return Content("❌ Không hợp lệ");

        c.TrangThai = TrangThai.DangDi;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
    }

    // ===== HOÀN THÀNH =====
    public IActionResult HoanThanh(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        var chuyen = _context.ChuyenDis.Find(id);

        if (chuyen == null || chuyen.TaiXeId != userId)
            return Content("❌ Không hợp lệ");

        chuyen.TrangThai = TrangThai.HoanThanh;

        var tt = new ThanhToan
        {
            ChuyenDiId = id,
            SoTien = chuyen.GiaDuKien,
            TrangThai = "ChuaTT"
        };

        _context.ThanhToans.Add(tt);
        _context.SaveChanges();

        _hub.Clients.Group(id.ToString())
            .SendAsync("NhanCapNhat", new { trangThai = "HoanThanh" });

        return RedirectToAction("QR", "ThanhToan", new { rideId = id });
    }
}



