function updateCartAjax(id, quantity) {
	var status = true;

	$.ajax({
		url: '/Order/AddToCart',
		type: 'POST',
		data: { productId: id, quantity: quantity },
		success: function (response) {
			//Handle
		},
		error: function (xhr, status, error) {
			status = false;
		}
	});

	return status;
}

function checkCouponAjax(couponCode) {
	$.ajax({
		url: '/Coupon/CheckCoupon',
		type: 'POST',
		data: { couponCode: couponCode },
		success: function (response) {
			if (response.success) {
				showToast("Appy coupon success");

				var coupon = JSON.parse(response.data);
				document.getElementById("coupon-id").value = coupon.CouponId;

				applyCoupon(coupon);

			} else {
				showToast("Coupon was not exist");
				document.querySelector(".discount").innerHTML = formatCurrency(0);
				loadContent();
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to apply coupon")
		}
	});
}

function applyCoupon(coupon) {
	let subTotal = convertCurrency(document.querySelector(".subTotal").innerHTML);

	if (coupon.MinimumOrderAmount != null && coupon.MinimumOrderAmount > subTotal) {
		showToast("Subtotal must more than " + coupon.MinimumOrderAmount);
		return;
	}

	let discount = 0;

	if (coupon.DiscountValue != null) {
		discount = coupon.DiscountValue;
	}
	else {
		let value = subTotal * (coupon.DiscountPercentage) / 100;
		let discount = !coupon.MaximumDiscountAmount ? value :
			(value > coupon.MaximumDiscountAmount ? coupon.MaximumDiscountAmount : value);
	}

	document.querySelector(".discount").innerHTML = formatCurrency(discount);
	loadContent();
}