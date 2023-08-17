using IB_projekat.Certificates.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using IB_projekat.Users.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using Microsoft.AspNetCore.Authentication.Certificate;
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
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<IB_projekat.DatabaseContext>(options =>
{
    options.UseNpgsql("Server=localhost;Database=IB;User Id=erdel;Password=admin;");
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.ClearProviders()));
}, ServiceLifetime.Transient);
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
    options.AddPolicy("Policy",builder =>
    {
        builder
            .WithOrigins("http://localhost:3000", "https://accounts.google.com")
            .AllowAnyHeader()
            .AllowAnyMethod().AllowCredentials();
    });
});
/*builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(8000, listenOptions =>
    {
        var certificatePath = "certs/00EAF97019E9753ADD.crt";
        var privateKeyPath = "keys/00EAF97019E9753ADD.key";
        var certificatePassword = "YourCertificatePassword";

        X509Certificate2 cert = new X509Certificate2($"certs/00EAF97019E9753ADD.crt"); ;
        using (RSA rsa = RSA.Create())
        {
            try
            {
                rsa.ImportRSAPrivateKey(File.ReadAllBytes($"keys/00EAF97019E9753ADD.key"), out _);
                RSA publicKey = cert.GetRSAPublicKey();
                cert = cert.CopyWithPrivateKey(rsa);
            }
            catch (Exception ex)

            {
            }
        }
        listenOptions.UseHttps(cert);
    });
});*/
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
        options.Cookie.SameSite = SameSiteMode.None;
        options.SlidingExpiration = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    }).AddGoogle(options =>
    {
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ClientId = "416218689606-jqr5uspc77jn3ltifraq6gpquuhloof3.apps.googleusercontent.com";
        options.ClientSecret = "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";

        options.CallbackPath = "/api/user/handle-google-login";
        options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
        options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
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

var logger = new LoggerConfiguration()
.ReadFrom.Configuration(configuration)
.Enrich.FromLogContext()
.CreateLogger();
builder.Services.AddSingleton<Serilog.ILogger>(logger);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new CustomMediaTypeInputFormatter("application/x-x509-ca-cert"));
    options.OutputFormatters.Insert(0, new CustomMediaTypeOutputFormatter("application/x-x509-ca-cert"));
});


var app = builder.Build();

app.UseCors("Policy");

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
