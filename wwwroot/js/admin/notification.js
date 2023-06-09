//var audio = new Audio("noti.mp3");
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/noti")
    .build();

connection.start().then(function () {
    console.log("Connect signalr success");
    connection.invoke("JoinRoom", "employee");
})

connection.on("ReceiveOrder", message => {
    //audio.play();
    alert(message);

    if (window.location.href.endsWith("bill")) {
        GetBillData();
    }
})

function GetBillData() {
    let container = document.getElementById("dataContainer");

    $.ajax({
        url: '/Admin/Bill/GetBillData',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (result) { container.innerHTML = result }
    });
}