
using Baitap2.Data;
using Baitap2.Hubs;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class KhachController : Controller
{
    private readonly DemoContext _context;
    private readonly IHubContext<RideHub> _hub;

    public KhachController(DemoContext context, IHubContext<RideHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    public IActionResult DatChuyen()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DatChuyen(ChuyenDi c, double km)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return Content("❌ Chưa login");

        c.KhachId = userId.Value;
        c.ThoiGianDat = DateTime.Now;
        c.CreatedAt = DateTime.Now;

        // 💰 tính tiền
        c.GiaDuKien = (decimal)(10000 + km * 8000);

        // luôn bắt đầu = tìm tài xế
        c.TrangThai = TrangThai.DangTimTaiXe;

        _context.ChuyenDis.Add(c);
        _context.SaveChanges();

        // 🔥 gửi đúng tài xế theo loại xe
        var taiXeIds = _context.TaiXes
            .Where(tx => tx.Online && tx.LoaiXe == c.LoaiXe)
            .Select(tx => tx.NguoiDungId.ToString())
            .ToList();

        await _hub.Clients.Users(taiXeIds)
            .SendAsync("CoChuyenMoi", new
            {
                chuyenId = c.Id
            });

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
        int? userId = HttpContext.Session.GetInt32("UserId");

        var chuyen = _context.ChuyenDis.FirstOrDefault(x => x.Id == id);

        if (chuyen == null)
            return NotFound();

        // 🔥 chỉ khách của chuyến mới được hủy
        //if (chuyen.KhachId != userId)
        //    return Content("❌ Không có quyền");

        // 🔥 chỉ cho hủy khi chưa có tài xế hoặc đang tìm
        if (chuyen.TrangThai != TrangThai.DangTimTaiXe)
            return Content("❌ Không thể hủy lúc này");

        chuyen.TrangThai = TrangThai.DaHuy;

        _context.SaveChanges();

        await _hub.Clients.Group(id.ToString())
            .SendAsync("NhanCapNhat", new
            {
                chuyenId = id,
                trangThai = "DaHuy"
            });

        return RedirectToAction("DatChuyen");
    }
}