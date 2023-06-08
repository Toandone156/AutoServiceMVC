var audio = new Audio("noti.mp3");
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
    audio.play();
    showToast(message);
})