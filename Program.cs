using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
//Add database context
services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
    options.UseLoggerFactory(LoggerFactory.Create(builder =>
    {
        builder
        .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information)
        .AddConsole();
    }));
});

services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "vi-VN", "en-US" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

//Add service
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

services.AddHttpContextAccessor();
services.AddScoped<IImageUploadService, ImageUploadService>();

//Add enviroment variable
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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

#region currency
var supportedCultures = new[] { "vi-VN", "en-US" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
#endregion


app.UseAuthorization();

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
