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
		showToast("Subtotal must more than " + formatCurrency(coupon.MinimumOrderAmount));
		return;
	}

	let discount = 0;

	if (coupon.DiscountValue != null) {
		discount = coupon.DiscountValue;
	}
	else {
		let value = subTotal * (coupon.DiscountPercentage) / 100;
		discount = coupon.MaximumDiscountAmount == null ? value :
			(value > coupon.MaximumDiscountAmount ? coupon.MaximumDiscountAmount : value);
	}

	showToast("Apply coupon success")
	document.querySelector(".discount").innerHTML = formatCurrency(discount);
	loadContent();
}

function tradeCouponAjax(coupon) {
	var couponId = coupon.getAttribute("data-value");

	$.ajax({
		url: '/Coupon/TradeCoupon',
		type: 'POST',
		data: { id: couponId },
		success: function (response) {
			if (response.success) {
				showToast("Trade coupon success.");
				coupon.remove();
			} else {
				showToast(response.message);
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to apply coupon")
		}
	});
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
	var bookElement = document.querySelector(".booktable");

	$.ajax({
		url: '/Order/AccessTableApi',
		type: 'POST',
		data: { tablecode: tablecode },
		success: function (response) {
			if (response.success) {
				console.log(response)
				showToast(response.message);

				notbookElement.classList.add('d-none');
				bookElement.innerHTML = `Your table: ${response.name} <a href="#" class="exit-table text-white" onclick="event.preventDefault(); exitTableAjax();"><i class="ion-ios-log-out"></i></a>`
				bookElement.classList.remove('d-none');
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

function exitTableAjax() {
	var notbookElement = document.querySelector(".notbooktable");
	var bookElement = document.querySelector(".booktable");

	$.ajax({
		url: '/Order/ExitTableApi',
		type: 'POST',
		data: { },
		success: function (response) {
			if (response.success) {
				console.log(response)
				showToast("Exit table success");


				bookElement.classList.add('d-none');
				notbookElement.classList.remove('d-none');
			} else {
				showToast("Exit table fail");
				return null;
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to send api")
		}
	});
}

function BotAddToCartApi(id, quantity) {
	$.ajax({
		url: '/Product/DetailApi',
		type: 'POST',
		data: { id: id },
		success: function (response) {
			return AddItemToCart(id, response.name, response.image, quantity);
		},
		error: function (xhr, status, error) {
			showToast("Fail to send api")
		},
		async: false
	});
}