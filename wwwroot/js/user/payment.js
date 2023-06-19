var products = document.getElementsByClassName("order-item");
var subTotal = document.querySelector(".subTotal");
var discount = document.querySelector(".discount");
var totalItem = document.querySelector(".total");
var totalInput = document.getElementById("input-total");

function loadContent() {

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
	var inputField = orderItem.querySelector('input');
	var quantity = parseInt(inputField.value, 10);

	quantity += increment;

	if (quantity === 0) {
		return;
	} else {
		inputField.value = quantity;

		var priceElement = orderItem.querySelector('.price');
		var price = convertCurrency(priceElement.innerHTML);
		var productTotalElement = orderItem.querySelector('.productTotal');
	}

	updateCartAjax(orderItem.getAttribute("data-id"), quantity);
	loadContent();
}

var quantity = document.querySelectorAll("#quantity");

var minusButtons = document.querySelectorAll('.quantity-left-minus');
var plusButtons = document.querySelectorAll('.quantity-right-plus');
var deleteButtons = document.querySelectorAll('.remove-payment-item');

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
		var item = button.closest(".order-item");
		updateCartAjax(item.getAttribute("data-id"), 0);
		item.remove();

		loadContent();
	})
})

quantity.forEach(p => {
	p.addEventListener('keyup', loadContent);
})

loadContent();

//Coupon check
var typingTimer;
var doneTypingInterval = 1500;
var input = document.getElementById("coupon-code");
var loading = document.querySelector(".coupon-loading");

if (input != null) {
	input.addEventListener("keyup", e => {
		loading.classList.add("active");
		clearTimeout(typingTimer);

		if (input.value) {
			typingTimer = setTimeout(() => {
				checkCouponAjax(input.value);
				loading.classList.remove("active");
			}, doneTypingInterval);
		} else {
			loading.classList.remove("active");
			document.querySelector(".discount").innerHTML = formatCurrency(0);
			loadContent();
		}
	})

	input.addEventListener("keydown", e => {
		clearTimeout(typingTimer);
	})
}

var paymentButton = document.querySelector(".paymentbutton");

paymentButton.addEventListener("click", e => {
	if (products.length == 0) {
		showToast("Please add some product before payment");
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
						window.location.assign('/auth/login');
					}
				}
			}
		});
	}
})