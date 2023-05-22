using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceMVC.Migrations
{
    public partial class AddProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Employees_CreatorID",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_UserTypes_UserTypeID",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Coupons_ApplyCouponID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tables_TableID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Employees_EmployeeID",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Orders_OrderID",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Status_StatusID",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTrading_Users_UserID",
                table: "PointTrading");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeedbacks_Products_ProductID",
                table: "ProductFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeedbacks_Users_UserID",
                table: "ProductFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFeedbacks_Users_UserID",
                table: "ServiceFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCoupons_Coupons_CouponID",
                table: "UserCoupons");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCoupons_Users_UserID",
                table: "UserCoupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTypes_UserTypeID",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserTypeID",
                table: "Users",
                newName: "UserTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserTypeID",
                table: "Users",
                newName: "IX_Users_UserTypeId");

            migrationBuilder.RenameColumn(
                name: "CouponID",
                table: "UserCoupons",
                newName: "CouponId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserCoupons",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCoupons_CouponID",
                table: "UserCoupons",
                newName: "IX_UserCoupons_CouponId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ServiceFeedbacks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceFeedbacks_UserID",
                table: "ServiceFeedbacks",
                newName: "IX_ServiceFeedbacks_UserId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ProductFeedbacks",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductFeedbacks",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductFeedbackID",
                table: "ProductFeedbacks",
                newName: "ProductFeedbackId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeedbacks_UserID",
                table: "ProductFeedbacks",
                newName: "IX_ProductFeedbacks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeedbacks_ProductID",
                table: "ProductFeedbacks",
                newName: "IX_ProductFeedbacks_ProductId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "PointTrading",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PointTrading_UserID",
                table: "PointTrading",
                newName: "IX_PointTrading_UserId");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "OrderStatus",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "OrderStatus",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "OrderStatus",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_StatusID",
                table: "OrderStatus",
                newName: "IX_OrderStatus_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_OrderID",
                table: "OrderStatus",
                newName: "IX_OrderStatus_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_EmployeeID",
                table: "OrderStatus",
                newName: "IX_OrderStatus_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TableID",
                table: "Orders",
                newName: "TableId");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodID",
                table: "Orders",
                newName: "PaymentMethodId");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Orders",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "ApplyCouponID",
                table: "Orders",
                newName: "ApplyCouponId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_TableID",
                table: "Orders",
                newName: "IX_Orders_TableId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentMethodID",
                table: "Orders",
                newName: "IX_Orders_PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EmployeeID",
                table: "Orders",
                newName: "IX_Orders_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ApplyCouponID",
                table: "Orders",
                newName: "IX_Orders_ApplyCouponId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "OrderDetails",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Employees",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RoleID",
                table: "Employees",
                newName: "IX_Employees_RoleId");

            migrationBuilder.RenameColumn(
                name: "UserTypeID",
                table: "Coupons",
                newName: "UserTypeId");

            migrationBuilder.RenameColumn(
                name: "CreatorID",
                table: "Coupons",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CouponID",
                table: "Coupons",
                newName: "CouponId");

            migrationBuilder.RenameIndex(
                name: "IX_Coupons_UserTypeID",
                table: "Coupons",
                newName: "IX_Coupons_UserTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Coupons_CreatorID",
                table: "Coupons",
                newName: "IX_Coupons_CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Employees_CreatorId",
                table: "Coupons",
                column: "CreatorId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_UserTypes_UserTypeId",
                table: "Coupons",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Coupons_ApplyCouponId",
                table: "Orders",
                column: "ApplyCouponId",
                principalTable: "Coupons",
                principalColumn: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_EmployeeId",
                table: "Orders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tables_TableId",
                table: "Orders",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Employees_EmployeeId",
                table: "OrderStatus",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Orders_OrderId",
                table: "OrderStatus",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Status_StatusId",
                table: "OrderStatus",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTrading_Users_UserId",
                table: "PointTrading",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeedbacks_Products_ProductId",
                table: "ProductFeedbacks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeedbacks_Users_UserId",
                table: "ProductFeedbacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFeedbacks_Users_UserId",
                table: "ServiceFeedbacks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCoupons_Coupons_CouponId",
                table: "UserCoupons",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "CouponId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCoupons_Users_UserId",
                table: "UserCoupons",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTypes_UserTypeId",
                table: "Users",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Employees_CreatorId",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_UserTypes_UserTypeId",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Coupons_ApplyCouponId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_EmployeeId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tables_TableId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Employees_EmployeeId",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Orders_OrderId",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatus_Status_StatusId",
                table: "OrderStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTrading_Users_UserId",
                table: "PointTrading");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeedbacks_Products_ProductId",
                table: "ProductFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeedbacks_Users_UserId",
                table: "ProductFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceFeedbacks_Users_UserId",
                table: "ServiceFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCoupons_Coupons_CouponId",
                table: "UserCoupons");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCoupons_Users_UserId",
                table: "UserCoupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserTypes_UserTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "Users",
                newName: "UserTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                newName: "IX_Users_UserTypeID");

            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "UserCoupons",
                newName: "CouponID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserCoupons",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserCoupons_CouponId",
                table: "UserCoupons",
                newName: "IX_UserCoupons_CouponID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ServiceFeedbacks",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceFeedbacks_UserId",
                table: "ServiceFeedbacks",
                newName: "IX_ServiceFeedbacks_UserID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductFeedbacks",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductFeedbacks",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "ProductFeedbackId",
                table: "ProductFeedbacks",
                newName: "ProductFeedbackID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeedbacks_UserId",
                table: "ProductFeedbacks",
                newName: "IX_ProductFeedbacks_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeedbacks_ProductId",
                table: "ProductFeedbacks",
                newName: "IX_ProductFeedbacks_ProductID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PointTrading",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_PointTrading_UserId",
                table: "PointTrading",
                newName: "IX_PointTrading_UserID");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "OrderStatus",
                newName: "StatusID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderStatus",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "OrderStatus",
                newName: "EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_StatusId",
                table: "OrderStatus",
                newName: "IX_OrderStatus_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_OrderId",
                table: "OrderStatus",
                newName: "IX_OrderStatus_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStatus_EmployeeId",
                table: "OrderStatus",
                newName: "IX_OrderStatus_EmployeeID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "Orders",
                newName: "TableID");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                table: "Orders",
                newName: "PaymentMethodID");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Orders",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "ApplyCouponId",
                table: "Orders",
                newName: "ApplyCouponID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_TableId",
                table: "Orders",
                newName: "IX_Orders_TableID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders",
                newName: "IX_Orders_PaymentMethodID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders",
                newName: "IX_Orders_EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ApplyCouponId",
                table: "Orders",
                newName: "IX_Orders_ApplyCouponID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderDetails",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductID");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Employees",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                newName: "IX_Employees_RoleID");

            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "Coupons",
                newName: "UserTypeID");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Coupons",
                newName: "CreatorID");

            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "Coupons",
                newName: "CouponID");

            migrationBuilder.RenameIndex(
                name: "IX_Coupons_UserTypeId",
                table: "Coupons",
                newName: "IX_Coupons_UserTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Coupons_CreatorId",
                table: "Coupons",
                newName: "IX_Coupons_CreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Employees_CreatorID",
                table: "Coupons",
                column: "CreatorID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_UserTypes_UserTypeID",
                table: "Coupons",
                column: "UserTypeID",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleID",
                table: "Employees",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderID",
                table: "OrderDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductID",
                table: "OrderDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Coupons_ApplyCouponID",
                table: "Orders",
                column: "ApplyCouponID",
                principalTable: "Coupons",
                principalColumn: "CouponID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodID",
                table: "Orders",
                column: "PaymentMethodID",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tables_TableID",
                table: "Orders",
                column: "TableID",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Employees_EmployeeID",
                table: "OrderStatus",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Orders_OrderID",
                table: "OrderStatus",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatus_Status_StatusID",
                table: "OrderStatus",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTrading_Users_UserID",
                table: "PointTrading",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeedbacks_Products_ProductID",
                table: "ProductFeedbacks",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeedbacks_Users_UserID",
                table: "ProductFeedbacks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceFeedbacks_Users_UserID",
                table: "ServiceFeedbacks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCoupons_Coupons_CouponID",
                table: "UserCoupons",
                column: "CouponID",
                principalTable: "Coupons",
                principalColumn: "CouponID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCoupons_Users_UserID",
                table: "UserCoupons",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserTypes_UserTypeID",
                table: "Users",
                column: "UserTypeID",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
