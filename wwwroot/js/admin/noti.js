var connection = new signalR.HubConnectionBuilder()
    .withUrl("/noti")
    .build();

connection.start().then(function () {
    console.log("Connect signalr success");
    connection.invoke("JoinRoom", "employee");
})

connection.on("ReceiveNoti", (message, statusId) => {
    if (message !== "") {
        NotiAudio();
        showToast(message);
    }

    let url = window.location.href;
    if (url.includes("admin/bill/index/" + statusId)) {
        GetBillData(statusId);
    }
})

function GetBillData(id) {
    let container = document.getElementById("data-container");

    $.ajax({
        url: '/Admin/Bill/GetBillData',
        contentType: 'application/html ; charset:utf-8',
        data: { id: id},
        type: 'GET',
        dataType: 'html',
        success: function (result) { container.innerHTML = result }
    });
}

function NotiAudio() {
    let audio = new Audio("/noti.mp3");
    let button = document.createElement("button");
    button.classList.add("d-none");
    button.addEventListener("click", e => {
        e.preventDefault();
        audio.play();
    })

    button.click();
    button.remove();
}