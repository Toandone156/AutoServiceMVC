USE [AutoServiceDB]
GO
/****** Object:  Trigger [dbo].[UpdateOrderTotal]    Script Date: 25/05/2023 21:51:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateOrderTotal]
ON [dbo].[OrderDetails]
AFTER INSERT, UPDATE
AS
BEGIN
    -- Update total amount for each affected order
    UPDATE o
    SET o.amount = (
        SELECT SUM(od.Quantity * p.Price)
        FROM OrderDetails od
        INNER JOIN Products p ON p.ProductId = od.ProductId
        WHERE od.OrderId = o.OrderId
    )
    FROM dbo.Orders o
    WHERE o.OrderId IN (
        SELECT DISTINCT Inserted.OrderId
        FROM inserted -- Newly inserted or updated OrderDetail records
    )

	-- Apply coupon discount if available
    UPDATE o
    SET o.amount = CASE
        WHEN c.DiscountPercentage IS NOT NULL THEN o.Amount * (1 - c.DiscountPercentage * 0.01)
        WHEN c.DiscountValue IS NOT NULL THEN o.Amount - c.DiscountValue
        ELSE o.amount
    END
    FROM dbo.Orders o
    INNER JOIN inserted i ON o.OrderId = i.OrderId
    LEFT JOIN dbo.Coupons c ON o.ApplyCouponId = c.CouponId
END