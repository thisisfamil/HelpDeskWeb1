
using Microsoft.EntityFrameworkCore;
using HelpDeskWeb.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC servislerini elave et
builder.Services.AddControllersWithViews();

// SQLite database bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Database yarat
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();
}

// Middleware konfiqurasiyası
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
