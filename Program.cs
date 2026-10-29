using Microsoft.EntityFrameworkCore;
using QassimLabBooking.Data;
using QassimLabBooking.Models;

var builder = WebApplication.CreateBuilder(args);

// إضافة الـ Controllers مع معالجة الـ JSON Cycles
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 1. ربط قاعدة البيانات بـ PostgreSQL (Supabase) بدلاً من SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// إعدادات الـ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// إظهار صفحة تفاصيل الخطأ بوضوح لتشخيص المشكلة فوراً
app.UseDeveloperExceptionPage();

// 2. إنشاء قاعدة البيانات وزرع البيانات الأولية تلقائياً على السحاب
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // تطبيق الـ Migrations وضمان إنشائها على السحاب
    context.Database.EnsureCreated();

    if (!context.Labs.Any())
    {
        var lab1 = new Lab { Name = "3047" };
        context.Labs.Add(lab1);
        context.SaveChanges();

        context.Devices.AddRange(
            new Device { Name = "UV-Vis for liquid (1800)", LabId = lab1.Id },
            new Device { Name = "UV-Vis for liquid (1650)", LabId = lab1.Id },
            new Device { Name = "Photoreactor", LabId = lab1.Id },
            new Device { Name = "UV-Vis for solid", LabId = lab1.Id },
            new Device { Name = "IR Spectroscopy", LabId = lab1.Id }
        );
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();