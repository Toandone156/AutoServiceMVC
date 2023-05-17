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
    [Migration("20230517060700_AddModel")]
    partial class AddModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AutoServiceBE.Models.Category", b =>
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

            modelBuilder.Entity("AutoServiceBE.Models.Coupon", b =>
                {
                    b.Property<int>("CouponID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CouponID"), 1L, 1);

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatorID")
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

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserTypeID")
                        .HasColumnType("int");

                    b.Property<bool>("isForNewUser")
                        .HasColumnType("bit");

                    b.HasKey("CouponID");

                    b.HasIndex("CreatorID");

                    b.HasIndex("UserTypeID");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"), 1L, 1);

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("ApplyCouponID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<int>("PaymentMethodID")
                        .HasColumnType("int");

                    b.Property<int>("TableID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("ApplyCouponID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("PaymentMethodID");

                    b.HasIndex("TableID")
                        .IsUnique();

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AutoServiceBE.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("AutoServiceBE.Models.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.HasKey("OrderStatusId");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("OrderID");

                    b.HasIndex("StatusID");

                    b.ToTable("OrderStatus");
                });

            modelBuilder.Entity("AutoServiceBE.Models.PaymentMethod", b =>
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

            modelBuilder.Entity("AutoServiceBE.Models.PointTrading", b =>
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

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PointTradingId");

                    b.HasIndex("UserID");

                    b.ToTable("PointTrading");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOutOfStock")
                        .HasColumnType("bit");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("ProductRating")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AutoServiceBE.Models.ProductFeedback", b =>
                {
                    b.Property<int>("ProductFeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductFeedbackID"), 1L, 1);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<byte>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ProductFeedbackID");

                    b.HasIndex("ProductID");

                    b.HasIndex("UserID");

                    b.ToTable("ProductFeedbacks");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Role", b =>
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

            modelBuilder.Entity("AutoServiceBE.Models.ServiceFeedback", b =>
                {
                    b.Property<int>("ServiceFeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceFeedbackId"), 1L, 1);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ServiceFeedbackId");

                    b.HasIndex("UserID");

                    b.ToTable("ServiceFeedbacks");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Status", b =>
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

            modelBuilder.Entity("AutoServiceBE.Models.Table", b =>
                {
                    b.Property<int>("TableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TableId"), 1L, 1);

                    b.Property<string>("TableCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TableId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("AutoServiceBE.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("UserTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AutoServiceBE.Models.UserCoupon", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("CouponID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.HasKey("UserID", "CouponID");

                    b.HasIndex("CouponID");

                    b.ToTable("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceBE.Models.UserType", b =>
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

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.Property<int>("EmployeesEmployeeID")
                        .HasColumnType("int");

                    b.Property<int>("RolesRoleId")
                        .HasColumnType("int");

                    b.HasKey("EmployeesEmployeeID", "RolesRoleId");

                    b.HasIndex("RolesRoleId");

                    b.ToTable("EmployeeRole");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Coupon", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Employee", "Creator")
                        .WithMany("CreatedCoupons")
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.UserType", "UserType")
                        .WithMany("Coupons")
                        .HasForeignKey("UserTypeID");

                    b.Navigation("Creator");

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Order", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Coupon", "ApplyCoupon")
                        .WithMany()
                        .HasForeignKey("ApplyCouponID");

                    b.HasOne("AutoServiceBE.Models.Employee", "Employee")
                        .WithMany("CreatedOrders")
                        .HasForeignKey("EmployeeID");

                    b.HasOne("AutoServiceBE.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentMethodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.Table", "Table")
                        .WithOne("Order")
                        .HasForeignKey("AutoServiceBE.Models.Order", "TableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID");

                    b.Navigation("ApplyCoupon");

                    b.Navigation("Employee");

                    b.Navigation("PaymentMethod");

                    b.Navigation("Table");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceBE.Models.OrderDetail", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AutoServiceBE.Models.OrderStatus", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID");

                    b.HasOne("AutoServiceBE.Models.Order", "Order")
                        .WithMany("OrderStatuses")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.Status", "Status")
                        .WithMany("OrderStatuses")
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Order");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("AutoServiceBE.Models.PointTrading", b =>
                {
                    b.HasOne("AutoServiceBE.Models.User", "User")
                        .WithMany("PointTradings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Product", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("AutoServiceBE.Models.ProductFeedback", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Product", "Product")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.User", "User")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceBE.Models.ServiceFeedback", b =>
                {
                    b.HasOne("AutoServiceBE.Models.User", "User")
                        .WithMany("ServiceFeedbacks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoServiceBE.Models.User", b =>
                {
                    b.HasOne("AutoServiceBE.Models.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("AutoServiceBE.Models.UserCoupon", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Coupon", "Coupon")
                        .WithMany("UserCoupons")
                        .HasForeignKey("CouponID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.User", "User")
                        .WithMany("UserCoupons")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.HasOne("AutoServiceBE.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesEmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutoServiceBE.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AutoServiceBE.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Coupon", b =>
                {
                    b.Navigation("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Employee", b =>
                {
                    b.Navigation("CreatedCoupons");

                    b.Navigation("CreatedOrders");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("OrderStatuses");
                });

            modelBuilder.Entity("AutoServiceBE.Models.PaymentMethod", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Product", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("ProductFeedbacks");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Status", b =>
                {
                    b.Navigation("OrderStatuses");
                });

            modelBuilder.Entity("AutoServiceBE.Models.Table", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("AutoServiceBE.Models.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("PointTradings");

                    b.Navigation("ProductFeedbacks");

                    b.Navigation("ServiceFeedbacks");

                    b.Navigation("UserCoupons");
                });

            modelBuilder.Entity("AutoServiceBE.Models.UserType", b =>
                {
                    b.Navigation("Coupons");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
