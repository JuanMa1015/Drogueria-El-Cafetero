using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Drogueria_Elcafetero.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Drogueria_Elcafetero.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Drogueria_ElcafeteroContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Drogueria_ElcafeteroContext") ?? throw new InvalidOperationException("Connection string 'Drogueria_ElcafeteroContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Login/Inicio";
        option.LogoutPath = "/Salir/Inicio";
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde el servidor
    options.Cookie.IsEssential = true; // Necesario para evitar que se bloquee
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=IndexSinRegistrar}/{id?}");

app.Run();
