const products = document.getElementsByClassName("order-item");
const subTotal = document.querySelector(".subTotal");
const discount = document.querySelector(".discount");
const totalItem = document.querySelector(".total");
const totalInput = document.getElementById("input-total");

// Coupon support variables
let ProcessCouponTimeout;
const doneTypingInterval = 1500;
const doneUpdateCartInterval = 0;
const couponInput = document.getElementById("coupon-code");
const loading = document.querySelector(".coupon-loading");

function loadContent() {
	lastTotal = totalInput.value;
	// Display message
	if (products.length == 0) {
		document.getElementById("emptyMessage").classList.remove("d-none");
	} else {
		document.getElementById("emptyMessage").classList.add("d-none");
	}

	let total = 0;

	for (let item of products) {
		let price = convertCurrency(item.querySelector(".price").innerHTML);
		let quantity = item.querySelector("#quantity").value;
		let productTotal = price * quantity;

		total += productTotal;

		item.querySelector(".productTotal").innerHTML = formatCurrency(productTotal)
	}

	subTotal.innerHTML = formatCurrency(total);
	totalInput.value = (total - convertCurrency(discount.innerHTML) < 0) ? 0 : total - convertCurrency(discount.innerHTML);
	totalItem.innerHTML = formatCurrency(parseInt(totalInput.value));
}

function updateQuantity(element, increment) {
	let orderItem = element.closest(".order-item");
	let inputField = orderItem.querySelector('input');
	let quantity = parseInt(inputField.value, 10);

	quantity += increment;

	if (quantity === 0) {
		return;
	} else {
		inputField.value = quantity;
	}

	updateCartAjax(orderItem.getAttribute("data-id"), quantity);
	loadContent();

	// Process Coupon if has any update in cart
	ProcessCouponIfUpdateCart();
}

let quantity = document.querySelectorAll("#quantity");
let minusButtons = document.querySelectorAll('.quantity-left-minus');
let plusButtons = document.querySelectorAll('.quantity-right-plus');
let deleteButtons = document.querySelectorAll('.remove-payment-item');

minusButtons.forEach(function (button) {
	button.addEventListener('click', function () {
		updateQuantity(this, -1);
	});
});

plusButtons.forEach(function (button) {
	button.addEventListener('click', function () {
		updateQuantity(this, 1);
	});
});

deleteButtons.forEach(button => {
	button.addEventListener('click', function () {
		let orderItem = button.closest(".order-item");
		updateCartAjax(orderItem.getAttribute("data-id"), 0);
		orderItem.remove();
		loadContent();
		ProcessCouponIfUpdateCart();
	})
})

quantity.forEach(p => {
	p.addEventListener('keyup', function() {
		loadContent();
		ProcessCouponIfUpdateCart();
	});
})

loadContent();

//Coupon check
function ProcessCouponIfUpdateCart() {
	UpdateOrder();
	ProcessCoupon(doneUpdateCartInterval, false);
}

function ProcessCouponIfInput() {
	InputCoupon();
	ProcessCoupon(doneTypingInterval, true);
}

function CheckSimilarCouponCode() {
	let lastCachedCouponCode = cachedCoupon.CouponCode;
	let inputCouponCode = couponInput.value;
	return lastCachedCouponCode == inputCouponCode;
}

function ProcessCoupon(timeout, inputCoupon) {
	let isValid = couponInput.value && (products != null && products.length > 0);
	if (isValid) {
		if(inputCoupon) loading.classList.add("active");
		clearTimeout(ProcessCouponTimeout);

		// Timeout section
		let handler = () => {
			checkCouponAjax(couponInput.value);
			loading.classList.remove("active");
		};
		if(inputCoupon) ProcessCouponTimeout = setTimeout(handler, timeout);
		else if(hasUpdateOrder) {
			loading.classList.remove("active");
		 	if(cachedCoupon != null && CheckSimilarCouponCode()) applyCoupon(cachedCoupon);
		}

	}
	else {
		// Remove coupon-id from input
		document.getElementById("coupon-id").value = "";
		document.getElementById("coupon-id").removeAttribute("value");

		// Reset discount value to 0
		document.querySelector(".discount").innerHTML = formatCurrency(0);
		if(couponInput.value) showToast("Please add some products to apply coupon");
	}
}

if (couponInput != null) {
	couponInput.onkeydown = clearTimeout(ProcessCouponTimeout);

	couponInput.addEventListener("keyup", _ => {
		let isValid = couponInput.value && (products != null && products.length > 0);
		if (isValid) {
			loading.classList.add("active");
			clearTimeout(ProcessCouponTimeout);
			ProcessCouponIfInput();
		}
		else {
			loading.classList.remove("active");
			document.querySelector(".discount").innerHTML = formatCurrency(0);
			loadContent();
		}
	});
}

const paymentButton = document.querySelector(".paymentbutton");
paymentButton.addEventListener("click", e => {
	if (products.length == 0) {
		showToast("Please add some products before payment");
		e.preventDefault();
	} else if (document.querySelector(".booktable").classList.contains("d-none")) {
		showToast("Please BOOK TABLE before payment");
		opentable();
		e.preventDefault();
	} else if (document.querySelector('.nav-link[href="/auth/login"]') != null) {
		e.preventDefault();

		$.confirm({
			title: 'Not Login',
			content: 'Do you want login to apply coupon and get points after order done?',
			buttons: {
				confirm: {
					text: "Continue",
					btnClass: 'btn-orange',
					keys: ['enter'],
					action: function () {
						document.getElementById("goOrderForm").submit();
					}
				},
				loginn: {
					text: "Login now",
					action: function () {
						window.location.assign('@Url.ActionLink("Login", "Auth")');
					}
				}
			}
		});
	}
})