using Baitap2.Data;
using Microsoft.AspNetCore.Mvc;

public class ThanhToanController : Controller
{
    private readonly DemoContext _context;

    public ThanhToanController(DemoContext context)
    {
        _context = context;
    }

    // 👉 HIỂN THỊ QR
    public IActionResult QR(int rideId)
    {
        var tt = _context.ThanhToans
            .FirstOrDefault(x => x.ChuyenDiId == rideId);

        if (tt == null) return Content("Không có thanh toán");

        return View(tt);
    }

    

    public IActionResult XacNhan(int id)
    {
        var tt = _context.ThanhToans.Find(id);
        if (tt == null) return NotFound();

        tt.TrangThai = "DaTT";
        _context.SaveChanges();

        // 🔥 QUAY VỀ MÀN HÌNH NHẬN CUỐC
        return RedirectToAction("Index", "TaiXe");
    }

}
