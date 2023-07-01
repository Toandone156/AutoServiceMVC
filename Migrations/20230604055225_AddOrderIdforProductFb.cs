using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceMVC.Migrations
{
    public partial class AddOrderIdforProductFb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ProductFeedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeedbacks_OrderId",
                table: "ProductFeedbacks",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeedbacks_Orders_OrderId",
                table: "ProductFeedbacks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeedbacks_Orders_OrderId",
                table: "ProductFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeedbacks_OrderId",
                table: "ProductFeedbacks");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductFeedbacks");
        }
    }
}
