using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
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
services.AddScoped<ICommonRepository<UserCoupon>, UserCouponRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
