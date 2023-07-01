var connection = new signalR.HubConnectionBuilder()
    .withUrl("/noti")
    .build();

connection.start().then(function () {
    console.log("Connect signalr success");
    connection.invoke("JoinRoom", "user");
}).catch(err => {
    console.error(err.toString());
})

connection.on("ReceiveStatus", message => {
    NotiAudio();
    showToast(message);

    if (window.location.href.endsWith("bill")) {
        GetOrderData();
    }
})

function GetOrderData() {
    let container = document.getElementById("dataContainer");

    $.ajax({
        url: '/Bill/GetOrderData',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) { container.innerHTML = result }
    });
}

function NotiAudio() {
    let audio = new Audio("./noti.mp3");
    let button = document.createElement("button");
    button.classList.add("d-none");
    button.addEventListener("click", e => {
        e.preventDefault();
        audio.play();
    })

    button.click();
    button.remove();
}