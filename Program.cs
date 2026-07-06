using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Güvenlik: Kestrel Server bilgisini gizle
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("AdminCookies")
    .AddCookie("AdminCookies", options =>
    {
        options.LoginPath = "/adminler/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });
builder.Services.AddDbContext<MyWebPage.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyWebPage.Data.AppDbContext>();
    db.Database.EnsureCreated();
}

// Güvenlik Middleware'i: HTTP Header'larını sıkılaştırma
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY"); // Clickjacking koruması
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff"); // MIME-sniffing engelleme
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block"); // XSS filtrelemesini zorla
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin"); // Gereksiz referrer bilgisini gizle
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; img-src 'self' data: https:;");
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapAreaControllerRoute(
    name: "admin_area",
    areaName: "Admin",
    pattern: "adminler/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
