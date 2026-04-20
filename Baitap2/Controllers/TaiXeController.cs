


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

    // 🔥 gửi trạng thái
    private async Task GuiTrangThai(ChuyenDi c)
    {
        await _hub.Clients.Group(c.Id.ToString())
            .SendAsync("NhanCapNhat", new
            {
                trangThai = c.TrangThai.ToString(),
                 taiXeId = c.TaiXeId
            });
    }

    public IActionResult Index(int? id, int? ignoreId)
    {
        ChuyenDi c = null;

        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var taiXe = _context.TaiXes
            .FirstOrDefault(x => x.NguoiDungId == userId);

        if (taiXe == null)
            return RedirectToAction("Index", "TaiXe");

        // 🔥 nếu vừa từ chối → KHÔNG load cuốc
        if (ignoreId != null)
        {
            return View(null);
        }

        if (id != null)
        {
            c = _context.ChuyenDis.Find(id);
        }
        else
        {
            c = _context.ChuyenDis
    .Where(x => x.TrangThai == TrangThai.DangTimTaiXe
             && x.TaiXeId == null)
    .AsEnumerable() // 🔥 chuyển sang memory để so sánh chắc chắn
    .Where(x => x.LoaiXe == taiXe.LoaiXe)
    .OrderByDescending(x => x.Id)
    .FirstOrDefault();
        }

        return View(c);
    }

    public async Task<IActionResult> Nhan(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        int userId = HttpContext.Session.GetInt32("UserId").Value;

        var taiXe = _context.TaiXes
            .FirstOrDefault(x => x.NguoiDungId == userId);

        if (taiXe == null)
            return RedirectToAction("Index");

        // 🔥 CHẶN KHÁC LOẠI XE
        if ((int)c.LoaiXe != (int)taiXe.LoaiXe)
        {
            return RedirectToAction("Index"); // ❌ không cho nhận luôn
        }

        // 🔥 CHẶN ĐÃ CÓ NGƯỜI NHẬN
        if (c.TaiXeId != null)
        {
            return Content("❌ Cuốc đã có tài xế khác nhận");
        }

        c.TaiXeId = taiXe.Id;
        c.TrangThai = TrangThai.DaNhan;

        _context.SaveChanges();

        await GuiTrangThai(c);

        return RedirectToAction("Index", new { id });
    }
    public async Task<IActionResult> TuChoi(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        c.TrangThai = TrangThai.DangTimTaiXe;
        c.TaiXeId = null;

        _context.SaveChanges();

        await GuiTrangThai(c);

        // 🔥 QUAN TRỌNG: không nhận lại cuốc này
        return RedirectToAction("Index", new { ignoreId = id });
    }
    public async Task<IActionResult> Den(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        c.TrangThai = TrangThai.DaDen;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
    }

    public async Task<IActionResult> BatDau(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        c.TrangThai = TrangThai.DangDi;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
    }

    public IActionResult HoanThanh(int id)
    {
        var chuyen = _context.ChuyenDis.Find(id);
        if (chuyen == null) return NotFound();

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
    .SendAsync("NhanCapNhat", new
    {
        trangThai = "HoanThanh",
        taiXeId = chuyen.TaiXeId
    });
        return RedirectToAction("QR", "ThanhToan", new { rideId = id });
    }
}
