﻿// <auto-generated />
using System;
using AutoServiceMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutoServiceMVC.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230624071058_ChangeFKRelation")]
    partial class ChangeFKRelation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AutoServiceMVC.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Coupon", b =>
                {
                    b.Property<int>("CouponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CouponId"), 1L, 1);

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountPercentage")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountValue")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MaximumDiscountAmount")
                        .HasColumnType("int");

                    b.Property<int?>("MinimumOrderAmount")
                        .HasColumnType("int");

                    b.Property<int>("PointAmount")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("Remain")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("CouponId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HashPassword")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("ApplyCouponId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("ntext");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("ApplyCouponId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("TableId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("OrderStatusId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OrderId");

                    b.HasIndex("StatusId");

                    b.ToTable("OrderStatus");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"), 1L, 1);

                    b.Property<string>("PaymentMethodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.PointTrading", b =>
                {
                    b.Property<int>("PointTradingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PointTradingId"), 1L, 1);

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<string>("TradeDescription")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<DateTime>("TradedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PointTradingId");

                    b.HasIndex("UserId");

                    b.ToTable("PointTrading");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInStock")
                        .HasColumnType("bit");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("ProductImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("ProductRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SellerQuantity")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.ProductFeedback", b =>
                {
                    b.Property<int>("ProductFeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductFeedbackId"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasColumnType("ntext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<byte>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProductFeedbackId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductFeedbacks");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.ServiceFeedback", b =>
                {
                    b.Property<int>("ServiceFeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceFeedbackId"), 1L, 1);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ServiceFeedbackId");

                    b.HasIndex("UserId");

                    b.ToTable("ServiceFeedbacks");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusId"), 1L, 1);

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("StatusId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Table", b =>
                {
                    b.Property<int>("TableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TableId"), 1L, 1);

                    b.Property<string>("TableCode")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TableId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HashPassword")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<long>("TotalAmount")
                        .HasColumnType("bigint");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.UserCoupon", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CouponId")
                        .HasColumnType("int");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "CouponId");

                    b.HasIndex("CouponId");

                    b.ToTable("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.UserType", b =>
                {
                    b.Property<int>("UserTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserTypeId"), 1L, 1);

                    b.Property<int>("MinAmount")
                        .HasColumnType("int");

                    b.Property<string>("UserTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserTypeId");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Coupon", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Employee", "Creator")
                        .WithMany("CreatedCoupons")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.UserType", "UserType")
                        .WithMany("Coupons")
                        .HasForeignKey("UserTypeId");

                    b.Navigation("Creator");

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Employee", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Role", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Order", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Coupon", "ApplyCoupon")
                        .WithMany()
                        .HasForeignKey("ApplyCouponId");

                    b.HasOne("AutoServiceMVC.Models.Employee", "Employee")
                        .WithMany("CreatedOrders")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("AutoServiceMVC.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.Table", "Table")
                        .WithMany("Orders")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("ApplyCoupon");

                    b.Navigation("Employee");

                    b.Navigation("PaymentMethod");

                    b.Navigation("Table");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.OrderDetail", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.OrderStatus", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("AutoServiceMVC.Models.Order", "Order")
                        .WithMany("OrderStatuses")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.Status", "Status")
                        .WithMany("OrderStatuses")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Order");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.PointTrading", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.User", "User")
                        .WithMany("PointTradings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Product", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.ProductFeedback", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Order", "Order")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.Product", "Product")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.User", "User")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.ServiceFeedback", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.User", "User")
                        .WithMany("ServiceFeedbacks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.User", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.UserCoupon", b =>
                {
                    b.HasOne("AutoServiceMVC.Models.Coupon", "Coupon")
                        .WithMany("UserCoupons")
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceMVC.Models.User", "User")
                        .WithMany("UserCoupons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Coupon", b =>
                {
                    b.Navigation("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Employee", b =>
                {
                    b.Navigation("CreatedCoupons");

                    b.Navigation("CreatedOrders");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("OrderStatuses");

                    b.Navigation("ProductFeedbacks");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.PaymentMethod", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Product", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ProductFeedbacks");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Role", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Status", b =>
                {
                    b.Navigation("OrderStatuses");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.Table", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("PointTradings");

                    b.Navigation("ProductFeedbacks");

                    b.Navigation("ServiceFeedbacks");

                    b.Navigation("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceMVC.Models.UserType", b =>
                {
                    b.Navigation("Coupons");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
