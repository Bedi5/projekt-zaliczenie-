using Inwentaryzacja.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodanie ApplicationDbContext (Identity)
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

// Dodanie InwentaryzacjaContext (Twoja aplikacja)
var inwentaryzacjaConnectionString = builder.Configuration.GetConnectionString("InwentaryzacjaConnection")
    ?? throw new InvalidOperationException("Connection string 'InwentaryzacjaConnection' not found.");
builder.Services.AddDbContext<InwentaryzacjaContext>(options =>
    options.UseSqlServer(inwentaryzacjaConnectionString));

// Konfiguracja Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// **Dodanie Razor Pages**
builder.Services.AddRazorPages();

// Dodanie obs³ugi kontrolerów i widoków
builder.Services.AddControllersWithViews();

// Budowanie aplikacji
var app = builder.Build();

// Konfiguracja aplikacji
if (app.Environment.IsDevelopment())
{
    // Dodanie migracji tylko w trybie deweloperskim
    app.UseDeveloperExceptionPage(); // Mo¿esz zamieniæ lub dodaæ inne ustawienia dla debugowania
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Dodaj autoryzacjê
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// **Mapowanie Razor Pages**
app.MapRazorPages();

app.Run();
