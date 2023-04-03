using IB_projekat.Certificates.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IB_projekat.DatabaseContext>(options =>
    options.UseNpgsql("Server=localhost;Database=IB;User Id=erdel;Password=admin;"));

builder.Services.AddScoped<IUserRepository<User>, UserRepository<User>>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();


app.Run();
