using Microsoft.EntityFrameworkCore;
using Red_Social_Voluntarios.Controllers;
using Red_Social_Voluntarios.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddDbContext<ContextoDB>(opciones =>
{
    var stringConexion = builder.Configuration.GetConnectionString("ConexionDefault");
    opciones.UseSqlServer(stringConexion);
});
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

namespace Red_Social_Voluntarios
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Controlador_Usuario controller = new Controlador_Usuario();
        }
    }
}


