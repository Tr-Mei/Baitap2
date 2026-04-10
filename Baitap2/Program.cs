using Baitap2.Data;
using Baitap2.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 🔥 DB
builder.Services.AddDbContext<DemoContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 🔥 FIX SESSION CHUẨN
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// 🔥 QUAN TRỌNG: session phải nằm sau routing
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapHub<RideHub>("/rideHub");

app.Run();