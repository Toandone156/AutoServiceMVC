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

function tradeCouponAjax(couponId) {
	var returnValue = false;

	$.ajax({
		url: '/Coupon/TradeCoupon',
		type: 'POST',
		data: { id: couponId },
		success: function (response) {
			if (response.success) {
				showToast("Trade coupon success.");
				returnValue = true;
			} else {
				showToast(response.message);
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to apply coupon")
		}
	});

	return returnValue;
}

function changePasswordApi(mail) {
	var returnValue = false;

	$.ajax({
		url: '/Auth/ChangePassword',
		type: 'POST',
		data: { mail: mail },
		success: function (response) {
			if (response.success) {
				showToast(response.message);
				returnValue = true;
			} else {
				showToast(response.message);
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to send api.");
		}
	});

	return returnValue;
}

function accessTableAjax(tablecode) {
	var notbookElement = document.querySelector(".notbooktable");
	var bookContainer = document.querySelector(".book");

	$.ajax({
		url: '/Order/AccessTableApi',
		type: 'POST',
		data: { tablecode: tablecode },
		success: function (response) {
			if (response.success) {
				console.log(response)
				showToast(response.message);

				notbookElement.remove();
				var existTable = document.createElement("h3");
				existTable.innerHTML = "YOUR TABLE: " + response.name.toUpperCase();
				bookContainer.append(existTable);
			} else {
				showToast(response.message);
				return null;
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to apply coupon")
		}
	});
}