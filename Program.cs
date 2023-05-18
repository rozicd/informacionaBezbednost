using IB_projekat.Certificates.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using IB_projekat.Users.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using System.Configuration;
using System.Data;
using IB_projekat.Certificates.Service;
using IB_projekat.Requests.Service;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Requests.Repository;
using IB_projekat.ActivationTokens.Repository;
using IB_projekat.ActivationTokens.Service;
using IB_projekat.tools;
using IB_projekat.SmsVerification.Repository;
using IB_projekat.SmsVerification.Service;
using IB_projekat.PasswordResetTokens.Repository;
using IB_projekat.PasswordResetTokens.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<IB_projekat.DatabaseContext>(options =>
    options.UseNpgsql("Server=localhost;Database=IB;User Id=ognje;Password=admin;"), ServiceLifetime.Transient);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



builder.Services.AddScoped<IUserRepository<User>, UserRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IActivationTokenRepository, ActivationTokenRepository>();
builder.Services.AddScoped<IActivationTokenService, ActivationTokenService>();
builder.Services.AddScoped<ISmsVerificationRepository, SmsVerificationRepository>();
builder.Services.AddScoped<ISmsVerificationService, SmsVerificationService>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenService, PasswordResetTokenService>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("http://localhost:3000")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return Task.CompletedTask;
            }
        };
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole(UserType.Admin.ToString());
    });
    options.AddPolicy("AuthorizedOnly", policy =>
    {
        policy.RequireRole(UserType.Authorized.ToString(), UserType.Admin.ToString());
    });

});

var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new CustomMediaTypeInputFormatter("application/x-x509-ca-cert"));
    options.OutputFormatters.Insert(0, new CustomMediaTypeOutputFormatter("application/x-x509-ca-cert"));
});

var app = builder.Build();
app.UseCors();


// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
