


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

        if (id != null)
        {
            c = _context.ChuyenDis.Find(id);
        }
        else
        {
            // 🔥 CHỈ LẤY CUỐC ĐANG CHỜ TÀI XẾ
            c = _context.ChuyenDis
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.TrangThai == TrangThai.DangTimTaiXe);
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

    public async Task<IActionResult> Nhan(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        c.TrangThai = TrangThai.DaNhan;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
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

        // 🔥 TẠO THANH TOÁN
        var tt = new ThanhToan
        {
            ChuyenDiId = id,
            SoTien = chuyen.GiaDuKien,
            TrangThai = "ChuaTT"
        };

        _context.ThanhToans.Add(tt);
        _context.SaveChanges();

        // 🔥 realtime cho khách
        _hub.Clients.Group(id.ToString())
            .SendAsync("NhanCapNhat", new { trangThai = "HoanThanh" });

        // 👉 CHUYỂN SANG QR
        return RedirectToAction("QR", "ThanhToan", new { rideId = id });
    }

}


