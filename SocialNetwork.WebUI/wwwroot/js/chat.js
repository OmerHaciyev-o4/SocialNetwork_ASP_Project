"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (currentUser, message) {
    var imgPath = currentUser.imageUrl;
    if (imgPath == null) imgPath = "/images/defaultImage.png";

    var timeStr = "";
    var today = new Date();
    var sendDate = new Date(message.sendDate.toString());
    if (today.getFullYear() == sendDate.getFullYear() &&
        today.getMonth() == sendDate().getMonth() &&
        today.getDay() == sendDate().getDay()) {
        timeStr = `${today.getHours()}:${today.getMinutes()}`;
    }
    else {
        timeStr = `${sendDate.getFullYear()}${sendDate.getMonth()}/${sendDate.getDay()}  ${today.getHours()}:${today.getMinutes()}`;
    }

    let content = ` <div class="message-item">
                        <div class="message-user">
                            <figure class="avatar">
                                <img src="${imgPath}" alt="${currentUser.username}">
                            </figure>
                            <div>
                                <h5>${currentUser.username}</h5>
                                <div class="time">${timeStr}</div>
                            </div>
                        </div>
                        <div class="message-wrap">${message.message}</div>
                    </div>`;
    document.getElementById("messagePanel").innerHTML += content;

    var userId = "4";
    var messageContent = "hecne";
    var messageJSON = JSON.stringify(message);

    connection.invoke("SendMessage", userId, messageContent, messageJSON)
        .catch(function (err) {
            console.error(err.toString());
        });
});

connection.on("UpdateMessage", function (friendId) {
    var elems = document.querySelectorAll(".notReady");
    for (var i = 0; i < elems.length; i++) {
        var elem = elems[i];
        elem.classList.remove("text-black-50");
        elem.classList.remove("notReady");
        elem.classList.add("text-primary");
    }
});

connection.on("Connect", function (info) {
    console.log(info);

    //var li = document.createElement("li");
    //document.getElementById("messagesList").appendChild(li);
    //li.innerHTML = `<span style='color:green;'>${info}</span>`;
});
connection.on("Disconnect", function (info) {
    console.log(info);

    //var li = document.createElement("li");
    //document.getElementById("messagesList").appendChild(li);
    //li.innerHTML = `<span style='color:red;'>${info}</span>`;
});


connection.start().then(function () {
    console.log("SignalR Runing...");
}).catch(function (err) {
    return console.error(err.toString());
});