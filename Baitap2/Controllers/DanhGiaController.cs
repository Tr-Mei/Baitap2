using Baitap2.Data;
using Baitap2.Models;
using Microsoft.AspNetCore.Mvc;

public class DanhGiaController : Controller
{
    private readonly DemoContext _context;

    public DanhGiaController(DemoContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Create(int rideId)
    {
        return View(rideId);
    }
    [HttpPost]
    public IActionResult Create(DanhGia dg)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return Content("❌ Chưa đăng nhập");
        }

        dg.NguoiDungId = userId.Value;

        _context.DanhGias.Add(dg);
        _context.SaveChanges();

        return RedirectToAction("DatChuyen", "Khach");
    }

}
