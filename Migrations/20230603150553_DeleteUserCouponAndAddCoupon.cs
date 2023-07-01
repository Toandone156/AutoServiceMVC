using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceMVC.Migrations
{
    public partial class DeleteUserCouponAndAddCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "UserCoupons");

            migrationBuilder.AddColumn<int>(
                name: "Remain",
                table: "Coupons",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remain",
                table: "Coupons");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "UserCoupons",
                type: "datetime2",
                nullable: true);
        }
    }
}
