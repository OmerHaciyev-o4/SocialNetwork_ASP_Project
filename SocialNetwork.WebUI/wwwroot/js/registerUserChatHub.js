"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/registeruserchathub").build();

connection.on("Connect", function (info) {
    console.log(info);
});
connection.on("Disconnect", function (info) {

    console.log(info);
});

connection.start().then(function () {
    console.log("SignalR Runing...");
}).catch(function (err) {
    return console.error(err.toString());
});