using AutoServiceMVC.Models;
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
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add mutilple key
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.HasKey(p => new
                {
                    p.OrderId,
                    p.ProductId
                });
            });

            modelBuilder.Entity<UserCoupon>(e =>
            {
                e.HasKey(p => new
                {
                    p.UserId,
                    p.CouponId
                });
            });

            modelBuilder.Entity<FavoriteProduct>(e =>
            {
                e.HasKey(p => new
                {
                    p.ProductId,
                    p.UserId
                });
            });

            modelBuilder
                .Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .OnDelete(deleteBehavior: DeleteBehavior.SetNull);


            modelBuilder
                .Entity<Order>()
                .HasOne(e => e.Table)
                .WithMany(e => e.Orders)
                .OnDelete(deleteBehavior: DeleteBehavior.SetNull);
        }
    }
}
