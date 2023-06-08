var audio = new Audio("noti.mp3");
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/noti")
    .build();

connection.start().then(function () {
    console.log("Connect signalr success");
    connection.invoke("JoinRoom", "employee");
})

connection.on("ReceiveOrder", message => {
    audio.play();
    alert(message);
})