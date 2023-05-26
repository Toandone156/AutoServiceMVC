let openShopping = document.querySelector('.shopping');
let closeShopping = document.querySelector('.closeShopping');
let list = document.querySelector('.list');
let listCards = document.querySelector('.listCard');
let body = document.querySelector('body');
let total = document.querySelector('.total');
let cart = document.querySelector('.shopping i');

openShopping.addEventListener('click', () => {
    if (body.classList.contains('active')) {
        body.classList.remove('active');
    } else {
        body.classList.add('active');
    }
})
closeShopping.addEventListener('click', () => {
    body.classList.remove('active');
})

reloadCard();

function addToCart(productId, quantity) {
    // AJAX request to add product to the cart
    $.ajax({
        url: '/Admin/Order/AddToCart',
        type: 'POST',
        data: { productId: productId, quantity: quantity },
        success: function (response) {
            var liElement = document.querySelector('li[data-id="' + productId + '"]');

            if (liElement == null) {
                let newDiv = document.createElement('li');
                newDiv.setAttribute("data-id", response.id)
                newDiv.innerHTML = `
                                <div>${response.name}</div>
                                <div>${response.price}</div>
                                <div>
                                <button onclick="changeQuantity(${response.id}, 'minus')">-</button>
                                <div class="count">${response.quantity}</div>
                                <button onclick="changeQuantity(${response.id}, 'plus')">+</button>
                                </div>`;
                listCards.appendChild(newDiv);
            } else {
                if (quantity == 0) {
                    liElement.remove();
                } else {
                    var countElement = liElement.querySelector('.count');
                    countElement.textContent = quantity;
                }
            }

            reloadCard();
        },
        error: function (xhr, status, error) {
            // Handle the error response
            console.log('Error adding product to cart: ' + error);
        }
    });
}

function reloadCard() {
    let count = 0;
    let totalPrice = 0;

    for (var i = 0; i < listCards.children.length; i++) {
        let li = listCards.children[i];

        console.log(li);

        var price = parseFloat(li.children[1].textContent);
        var quantity = parseInt(li.querySelector(".count").textContent);

        var subtotal = price * quantity;

        totalPrice = totalPrice + subtotal;
        count = count + quantity;
    }

    total.innerText = totalPrice.toLocaleString();

    cart.setAttribute("data-value", count);
}

function changeQuantity(productId, type) {
    var liElement = document.querySelector('li[data-id="' + productId + '"]');
    var quantity = parseInt(liElement.querySelector('.count').textContent);

    if (type == 'plus') {
        addToCart(productId, quantity + 1);
    } else {
        addToCart(productId, quantity - 1);
    }
}