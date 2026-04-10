using Baitap2.Data;
using Baitap2.Hubs;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class KhachController : Controller
{
    private readonly DemoContext _context;
    private readonly IHubContext<RideHub> _hubContext;

    public KhachController(DemoContext context, IHubContext<RideHub> hub)
    {
        _context = context;
        _hubContext = hub;
    }

    public IActionResult DatChuyen()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DatChuyen(ChuyenDi c, double km)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return Content("❌ Chưa login");

        c.KhachId = userId.Value;
        c.TrangThai = TrangThai.DangTimTaiXe;

        // 💰 tính giá
        c.GiaDuKien = (decimal)(10000 + km * 8000);

        c.CreatedAt = DateTime.Now;

        _context.ChuyenDis.Add(c);
        _context.SaveChanges();

        return RedirectToAction("TheoDoi", new { id = c.Id });
    }

    public IActionResult TheoDoi(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        return View(c);
    }
    [HttpPost]
    public async Task<IActionResult> HuyChuyen(int id)
    {
        var chuyen = _context.ChuyenDis.FirstOrDefault(x => x.Id == id);

        if (chuyen == null)
            return NotFound();

        // 🚨 set trạng thái
        chuyen.TrangThai = TrangThai.DaHuy;

        _context.SaveChanges();

        // 🚀 gửi realtime cho tất cả
        await _hubContext.Clients.Group(id.ToString())
            .SendAsync("NhanCapNhat", new
            {
                trangThai = "DaHuy"
            });

        return RedirectToAction("DatChuyen");
    }

}