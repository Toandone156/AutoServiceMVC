using AutoServiceBE.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSet
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PointTrading> PointTrading { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeedback> ProductFeedbacks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceFeedback> ServiceFeedbacks { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add mutilple key
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.HasKey(p => new
                {
                    p.OrderID,
                    p.ProductID
                });
            });

            modelBuilder.Entity<UserCoupon>(e =>
            {
                e.HasKey(p => new
                {
                    p.UserID,
                    p.CouponID
                });
            });
        }
    }
}
