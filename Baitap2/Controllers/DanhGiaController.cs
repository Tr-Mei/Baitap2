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

    public IActionResult Create(int rideId)
    {
        ViewBag.RideId = rideId;
        return View();
    }

    [HttpPost]
    public IActionResult Create(DanhGia dg)
    {
        int userId = int.Parse(HttpContext.Session.GetString("UserId"));

        dg.NguoiDungId = userId;

        _context.DanhGias.Add(dg);
        _context.SaveChanges();

        return RedirectToAction("DatChuyen", "Khach");
    }
}
