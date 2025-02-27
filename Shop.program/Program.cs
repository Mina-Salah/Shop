using Microsoft.EntityFrameworkCore;
using shop.Data.Implementation;
using Shop.Data.Context;
using Shop.Entities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Razor runtime compilation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add DbContext to the services container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("oneConnection"))
);

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



var app = builder.Build();

// Configure the HTTP request pipeline
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
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();
