using ApiDesafio2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProyectoDbContext>(item =>
item.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/Usuarios/Login"; // Ruta a la que se redirige en caso de no estar autenticado
        options.AccessDeniedPath = "/api/Usuarios/Login"; // Ruta en caso de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Duración de la cookie
    });
builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<ProyectoDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
