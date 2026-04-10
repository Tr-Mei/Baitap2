


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
            c = _context.ChuyenDis
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.TrangThai != TrangThai.HoanThanh);
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

    public async Task<IActionResult> HoanThanh(int id)
    {
        var c = _context.ChuyenDis.Find(id);
        if (c == null) return RedirectToAction("Index");

        c.TrangThai = TrangThai.HoanThanh;
        _context.SaveChanges();

        await GuiTrangThai(c);
        return RedirectToAction("Index", new { id });
    }
}