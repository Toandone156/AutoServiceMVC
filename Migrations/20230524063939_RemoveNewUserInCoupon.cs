using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceMVC.Migrations
{
    public partial class RemoveNewUserInCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isForNewUser",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "CouponCode",
                table: "Coupons",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CouponCode",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AddColumn<bool>(
                name: "isForNewUser",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
