using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Drogueria_Elcafetero.Data;
//using Drogueria_Elcafetero.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Drogueria_ElcafeteroContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Drogueria_ElcafeteroContext") ?? throw new InvalidOperationException("Connection string 'Drogueria_ElcafeteroContext' not found.")));
//builder.Services.AddDbContext<Drogueria_ElcafeteroContext>(options =>
    //options.UseNpgsql(builder.Configuration.GetConnectionString("Drogueria_ElcafeteroContext") ?? throw new InvalidOperationException("Connection string 'Drogueria_ElcafeteroContext' not found.")));

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
