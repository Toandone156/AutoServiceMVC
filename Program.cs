using AutoServiceMVC.Data;
using AutoServiceMVC.Hubs;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

#region Session
services.AddSession();

services.AddSingleton<ISessionCustom, SessionCustom>();
#endregion

#region Mail
services.AddOptions();
services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
services.AddTransient<IMailService, MailService>();
#endregion

//Add enviroment variable
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

#region DbContext
services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
    options.UseLoggerFactory(LoggerFactory.Create(builder =>
    {
        builder
        .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
        .AddConsole();
    }));
}, ServiceLifetime.Singleton);
#endregion

#region Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "User_Scheme";
    options.DefaultScheme = "User_Scheme";
})
    .AddCookie("User_Scheme", options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Cookie.MaxAge = options.ExpireTimeSpan;
        options.SlidingExpiration = true;
    })
    .AddCookie("Admin_Scheme", options =>
    {
        options.LoginPath = "/admin/auth/login";
        options.AccessDeniedPath = "/admin/";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
		options.Cookie.MaxAge = options.ExpireTimeSpan;
	})
    .AddGoogle(options =>
    {
        var googleConfig = builder.Configuration.GetSection("ExtenalLogin:Google");
        options.ClientId = googleConfig["ClientID"];
        options.ClientSecret = googleConfig["ClientSecret"];
    });

services.AddSingleton<ICookieAuthentication, CookieAuthentication>();
#endregion

services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "vi-VN" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

#region AddService
services.AddScoped<IAuthenticateService<User>, UserRepository>();
services.AddScoped<IAuthenticateService<Employee>, EmployeeRepository>();
services.AddScoped<ICommonRepository<Category>, CategoryRepository>();
services.AddScoped<ICommonRepository<Coupon>, CouponRepository>();
services.AddScoped<ICommonRepository<Order>, OrderRepository>();
services.AddScoped<ICommonRepository<Product>, ProductRepository>();
services.AddScoped<ICommonRepository<OrderDetail>, OrderDetailRepository>();
services.AddScoped<ICommonRepository<OrderStatus>, OrderStatusRepository>();
services.AddScoped<ICommonRepository<PaymentMethod>, PaymentMethodRepository>();
services.AddScoped<ICommonRepository<PointTrading>, PointTradingRepository>();
services.AddScoped<ICommonRepository<ProductFeedback>, ProductFeedbackRepository>();
services.AddScoped<ICommonRepository<ServiceFeedback>, ServiceFeedbackRepository>();
services.AddScoped<ICommonRepository<Status>, StatusRepository>();
services.AddScoped<ICommonRepository<Table>, TableRepository>();
services.AddScoped<ICommonRepository<Role>, RoleRepository>();
services.AddScoped<ICommonRepository<UserCoupon>, UserCouponRepository>();
services.AddScoped<ICommonRepository<UserType>, UserTypeRepository>();
services.AddScoped<ICommonRepository<Employee>, EmployeeRepository>();
services.AddScoped<ICommonRepository<User>, UserRepository>();

services.AddScoped<IHashPassword, HashPassword>();
services.AddScoped<IJWTAuthentication, JWTAuthentication>();

services.AddHttpContextAccessor();
services.AddScoped<IImageUploadService, ImageUploadService>();

services.AddScoped<IPaymentService, PaymentService>();
services.AddScoped<IPointService, PointService>();
services.AddSingleton<ICookieService, CookieService>();

services.AddSingleton<IChatbotService, ChatbotService>();
#endregion

services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

#region currency
var supportedCultures = new[] { "vi-VN" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<HubServer>("/noti");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
