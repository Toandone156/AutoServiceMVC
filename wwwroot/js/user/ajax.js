let hasUpdateOrder = true;
let successCoupon = true;
let hasCouponInput = true;
let lastTotal = 0;
var cachedCoupon;

function InputCoupon() {
	hasCouponInput = true;
}

function UpdateOrder() {
	hasUpdateOrder = true;
}

function ConfirmUpdateOrder() {
	hasUpdateOrder = false;
}

function ConfirmInputCoupon() {
	hasCouponInput = false;
}

function ConfirmPopupToast() {
	successCoupon = false;
}

function ResetPopupToast() {
	successCoupon = true;
}

function updateCartAjax(id, quantity) {
	let status = true;

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
				console.log("Run here");
				let coupon = JSON.parse(response.data);
				// Cache the Coupon for later process
				cachedCoupon = coupon;
				applyCoupon(coupon);
			} else {
				showToast("Coupon was not exist");
				ResetCoupon();
				document.querySelector(".discount").innerHTML = formatCurrency(0);
				loadContent();
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to apply coupon")
		}
	});
}

function ResetCoupon() {
	// Remove coupon-id from input
	document.getElementById("coupon-id").value = "";
	document.getElementById("coupon-id").removeAttribute("value");

	// Reset discount value to 0
	document.querySelector(".discount").innerHTML = formatCurrency(0);
	loadContent();
}

function applyCoupon(coupon) {
	let subTotal = convertCurrency(document.querySelector(".subTotal").innerHTML);

	if (coupon.MinimumOrderAmount != null && coupon.MinimumOrderAmount > subTotal) {
		ResetCoupon();
		ResetPopupToast();
		console.log(lastTotal);
		if(totalInput.value > lastTotal) {
			lastTotal = totalInput.value;
			showToast("Subtotal must more than " + formatCurrency(coupon.MinimumOrderAmount));
		}
		return;
	}
	// If apply coupon success, reset last Total
	lastTotal = 0;
	// Set coupon id to input
	document.getElementById("coupon-id").value = coupon.CouponId;
	let discount = 0;

	if (coupon.DiscountValue != null) {
		discount = coupon.DiscountValue;
	}
	else {
		let discountValue = subTotal * (coupon.DiscountPercentage) / 100;
		let afterProcessValue = (discountValue > coupon.MaximumDiscountAmount) ? coupon.MaximumDiscountAmount : discountValue;
		discount = coupon.MaximumDiscountAmount == null ? discountValue : afterProcessValue;
	}

	if(hasCouponInput) {
		ConfirmInputCoupon();
		ConfirmPopupToast();
		showToast("Apply coupon success");
	}
	else if(hasUpdateOrder) {
		ConfirmUpdateOrder();
		if(successCoupon) {
			ConfirmPopupToast();
			showToast("Apply coupon success");
		}
	}

	document.querySelector(".discount").innerHTML = formatCurrency(discount);
	loadContent();
}

function tradeCouponAjax(coupon) {
	let couponId = coupon.getAttribute("data-value");

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
	let changeSuccess = false;

	$.ajax({
		url: '/Auth/ChangePassword',
		type: 'POST',
		data: { mail: mail },
		success: function (response) {
			if (response.success) {
				showToast(response.message);
				changeSuccess = true;
			} else {
				showToast(response.message);
			}
		},
		error: function (xhr, status, error) {
			showToast("Fail to send api.");
		}
	});

	return changeSuccess;
}

function accessTableAjax(tablecode) {
	let notebookElement = document.querySelector(".notbooktable");
	let bookElement = document.querySelector(".booktable");

	$.ajax({
		url: '/Order/AccessTableApi',
		type: 'POST',
		data: { tablecode: tablecode },
		success: function (response) {
			if (response.success) {
				console.log(response)
				showToast(response.message);

				notebookElement.classList.add('d-none');
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
	let notebookElement = document.querySelector(".notbooktable");
	let bookElement = document.querySelector(".booktable");

	$.ajax({
		url: '/Order/ExitTableApi',
		type: 'POST',
		data: { },
		success: function (response) {
			if (response.success) {
				console.log(response)
				showToast("Exit table success");


				bookElement.classList.add('d-none');
				notebookElement.classList.remove('d-none');
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