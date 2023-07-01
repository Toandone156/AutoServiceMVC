var sendForm = document.querySelector('#chatform'),
    textInput = document.querySelector('.chatbox'),
    chatList = document.querySelector('.chatlist'),
    userBubble = document.querySelectorAll('.userInput'),
    botBubble = document.querySelectorAll('.bot__output'),
    animateBotBubble = document.querySelectorAll('.bot__input--animation'),
    overview = document.querySelector('.chatbot__overview'),
    animationCounter = 1,
    animationBubbleDelay = 600,
    input,
    chatbotButton = document.querySelector(".submit-button")

const sleep = (ms) => {
    return new Promise(resolve => setTimeout(resolve, ms));
}
sendForm.onkeydown = function (e) {
    if (e.keyCode == 13) {
        e.preventDefault();

        //No mix ups with upper and lowercases
        var input = textInput.value;

        //Empty textarea fix
        if (input.length > 0) {
            createBubble(input)
        }
    }
};

sendForm.addEventListener('submit', function (e) {
    //so form doesnt submit page (no page refresh)
    e.preventDefault();

    var input = textInput.value;

    //Empty textarea fix
    if (input.length > 0) {
        createBubble(input)
    }
}) //end of eventlistener

var createBubble = function (input) {
    //create input bubble
    var chatBubble = document.createElement('li');
    chatBubble.classList.add('userInput');

    //adds input of textarea to chatbubble list item
    chatBubble.innerHTML = input;

    //adds chatBubble to chatlist
    chatList.appendChild(chatBubble)

    textInput.value = "";

    //Sets chatlist scroll to bottom
    setTimeout(function () {
        chatList.scrollTop = chatList.scrollHeight;
    }, 0)

    connection.invoke("AskChatbot", input);
}

connection.on("ChatbotAnswer", async answer => {
    let message = JSON.parse(answer);

    debugger

    if (message.function_call) {
        let name = message.function_call.name;
        let botContent = [];

        if (name == "add_to_cart") {
            let arguments = JSON.parse(message.function_call.arguments);

            let orderdetails = arguments.orderdetails;
            for (let detail of orderdetails) {
                let response;

                $.ajax({
                    url: '/Product/DetailApi',
                    type: 'GET',
                    data: { id: detail.id },
                    success: function (res) {
                        response = res
                        botContent.push(AddItemToCart(detail.id, response.name, response.price, response.image, detail.quantity));
                    },
                    error: function (xhr, status, error) {
                        showToast("Fail to send api")
                    },
                    async: false
                });

                await sleep(500);
            }

            connection.invoke("AskChatbot", botContent + "");
        }
    } else {
        responseText(message.content);
    }
})



function responseText(e) {

    var response = document.createElement('li');

    response.classList.add('bot__output');

    //Adds whatever is given to responseText() to response bubble
    response.innerHTML = e;

    chatList.appendChild(response);

    //Sets chatlist scroll to bottom
    setTimeout(function () {
        chatList.scrollTop = chatList.scrollHeight;
    }, 0)
}
