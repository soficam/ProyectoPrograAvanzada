using ProyectoPrograAvanzada.Models;
using ProyectoPrograAvanzada.Filters;
using ProyectoPrograAvanzada.Services; // importa tu servicio
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Aquí registras tu servicio
builder.Services.AddScoped<PagoService>();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/HttpError500");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // mejor para ver errores en dev
}

app.UseStatusCodePagesWithReExecute("/Error/StatusCode/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogo}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();