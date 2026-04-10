using Baitap2.Data;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;

public class KhachController : Controller
{
    private readonly DemoContext _context;

    public KhachController(DemoContext context)
    {
        _context = context;
    }

    public IActionResult DatChuyen()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DatChuyen(ChuyenDi c)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return Content("❌ Chưa login");
        }

        c.KhachId = userId.Value;
        c.TrangThai = TrangThai.DangTimTaiXe;
        c.GiaDuKien = 20000;
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
}
