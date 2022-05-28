//imgupload
//selectedData
//result.src = URL.createObjectURL(event.target.files[0]);
var uniqueIds = [];
var selectedDatas = [];

function RandomId() {
    var count = 0;
    var id = 0;
    while (true) {
        id = Math.floor(Math.random() * 54368);
        if (uniqueIds.indexOf(id) === -1) {
            uniqueIds.push(id);
            break;
        }
    }
    return id;
}

function UnSelectData(id) {
    var childNodes = document.getElementById('selectedData').childNodes;
    var searchedNode = null;
    for (var i = 0; i < childNodes.length; i++) {
        var node = childNodes[i];
        if (node.id == id) {
            searchedNode = node;
            break;
        }
    }
    for (var i = 0; i < selectedDatas.length; i++) {
        if (selectedDatas[i].name == searchedNode.name) {
            selectedDatas.splice(i, 1);
            uniqueIds.splice(i, 1);
            break;
        }
    }

    document.getElementById('selectedData').removeChild(searchedNode);
}

$("#imgupload").change(function (e) {
    selectedDatas = e.target.files;
    var content = "";
    for (var i = selectedDatas.length - 1; i >= 0 ; i--) {
        var data = selectedDatas[i];
        if (data != null) {
            var id = RandomId();
            var item = `<div class="card w-100 shadow-xss rounded-xxxl p-1 mt-2 flex-row flex-wrap" id="${id}" name="${data.name}">
                            <div class="card-body p-0 ps-2">`;
            var cstType = "";
            if (data.type.toString().split('/')[0] === "image") {
                cstType = "image";
            }
            else if (data.type.toString().split('/')[0] === "video") {
                cstType = "film";
            }

            item += `   <i class="fa fa-${cstType} fw-600 text-grey-500 card-body p-0 d-flex align-items-center text-decoration-none user-select-none" style="font-size: 25px;">&nbsp;<span class="ms-2" style="font-size: 18px;">${data.name}</span></i>
                    </div>
                    <div class="d-flex flex-row-reverse flex-wrap pe-2">
                        <i class="fa fa-times text-danger pointer user-select-none" style="font-size: 25px;" onclick="UnSelectData(${id})"></i>
                    </div>
                </div>`;
            content += item;
        }

        $("#selectedData").html(content);
    }
});

function SharePost() {
    //with function get hash tags !!!!!

    var postObj = {
        postMessage: "",
        HashTag: "",
        SharedUserId: 0,
        Status: true,
        DataList: []
    };

    var tempByteArray = [];
    for (var i = 0; i < selectedDatas.length; i++) {
        var data = selectedDatas[i];
        console.log(data);

        var reader = new FileReader();
        reader.readAsArrayBuffer(data);
        reader.onloadend = (evt) => {
            if (evt.target.readyState === FileReader.DONE) {
                const arrayBuffer = evt.target.result, array = new Uint8Array(arrayBuffer);
                for (const a of array) {
                    tempByteArray.push(a);
                }
                postObj.DataList.push(tempByteArray);
                console.log(tempByteArray);
                tempByteArray = [];
            }
        }
    }

    console.log(postObj.DataList);

    //$.ajax({
    //    url: `/Database/NewPost?post=${JSON.stringify(postObj)}`,
    //    method: "POST"
    //});
}































function calcuteDate(sendDate, today) {
    var year = today.getYear() - sendDate.getYear();
    if (year <= 0) {
        var month = today.getMonth() - sendDate.getMonth();
        if (month <= 0) {
            var day = today.getDay() - sendDate.getDay();
            if (day <= 0) {
                var hour = today.getHours() - sendDate.getHours();
                if (hour <= 0) {
                    var minute = today.getMinutes() - sendDate.getMinutes();
                    if (minute <= 0) {
                        var second = today.getSeconds() - sendDate.getSeconds();
                        if (second <= 0) {
                            return "Now";
                        }
                        return second + "s";
                    }
                    return minute + "m";
                }
                return hour + "h";
            }
            else if (day == 1) {
                return "Yesterday";
            }
            else if ((day / 7) % 2 == 1) {
                return (day / 7) + "w";
            }
            return day + "d";
        }
        return month + "m";
    }
    return year + "y";
}

function GetNotification() {
    setInterval(function () {
        var currentNotificationLength = document.getElementById("friendRequestNotPanel").children.length;

        $.ajax({
            url: "/Database/GetFriends",
            method: "GET",
            success: function (users) {
                var confirmFriendPanelCount = document.getElementById("confirmFriendPanel").children.length;
                if (confirmFriendPanelCount != users.length) {
                    var usersLength = users.length;
                    if (users.length > 4) { usersLength = 3; }

                    var content = "";

                    for (var i = 0; i < usersLength; i++) {
                        var imgPath = "";
                        if (users[i].imageUrl == null) { imgPath = `/images/defaultImage.png`; }
                        else { imgPath = users[i].imageUrl; }

                        let user = `
                        <div class="card-body bg-transparent-card d-flex p-3 bg-greylight ms-3 me-3 rounded-3 mb-3" id="${users[i].id}">
                            <figure class="avatar me-2 mb-0">
                                <img src="${imgPath}" alt="${users[i].username}" class="shadow-sm rounded-circle w45" />
                            </figure>
                            <h4 class="fw-700 text-grey-900 font-xssss mt-2 text-decoration-none">@${users[i].username}
                                <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">NaN</span>
                            </h4>
                            <a href="/Home/Profile" class="btn-round-sm bg-white ms-auto mt-2 text-decoration-none">
                                <span class="feather-chevron-right font-xss text-grey-900"></span>
                            </a>
                        </div>`

                        content += user;
                    }

                    document.getElementById("confirmFriendPanel").innerHTML = content;
                }
            },
            error: function (err) { }
        });

        $.ajax({
            url: "/Database/GetNotification",
            method: "GET",
            success: function (notifications) {
                var notCount = notifications.length;

                if (notCount == 0) {
                    document.getElementById("notBadge").innerHTML = "0";
                    document.getElementById("firstNotDot").classList.add('d-none');
                }
                else if (notCount > 9) {
                    document.getElementById("notBadge").innerHTML = "9+";
                    document.getElementById("firstNotDot").classList.remove('d-none');
                }
                else {
                    document.getElementById("notBadge").innerHTML = notCount.toString();
                    document.getElementById("firstNotDot").classList.remove('d-none');
                }

                if (notifications.length > 0 && currentNotificationLength != notifications.length) {
                    $.ajax({
                        url: "/Database/GetUsers",
                        method: "GET",
                        success: function (users) {
                            var content = '';

                            var notLength = notifications.length;
                            if (notifications.length > 4) {
                                notLength = 3;
                            }

                            for (var i = 0; i < notLength; i++) {
                                if (notifications[i].title == "Friend Request") {
                                    for (var j = 0; j < users.length; j++) {
                                        if (users[j].id == notifications[i].senderUserId) {

                                            var imgPath = "";
                                            if (users[j].imageUrl == null) {
                                                imgPath = `/images/defaultImage.png`;
                                            } else {
                                                imgPath = users[j].imageUrl;
                                            }

                                            var sendTimestr = calcuteDate(new Date(notifications[i].sendDate.toString()), new Date());

                                            var request = `<div class="wrap">
                                                              <div class="card-body d-flex pt-0 ps-4 pe-4 pb-0 bor-0">
                                                                  <figure class="avatar me-3">
                                                                      <img src="${imgPath}" alt="${users[j].username}" class="shadow-sm rounded-circle w45"/>
                                                                  </figure>
                                                                  <h4 class="fw-700 text-grey-900 font-xssss mt-1">@${users[j].username}
                                                                      <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">${sendTimestr} before</span>
                                                                  </h4>
                                                              </div>
                                                              <div class="card-body d-flex align-items-center pt-0 ps-4 pe-4 pb-4">
                                                                  <a href="/Database/AddFriend?id=${users[j].id}&notId=${notifications[i].id}" class="p-2 lh-20 w100 bg-primary-gradiant me-2 text-white text-center font-xssss fw-600 ls-1 rounded-xl text-decoration-none">Confirm</a>
                                                                  <a href="/Database/RemoveNotification?notId=${notifications[i].id}" class="p-2 lh-20 w100 bg-grey text-grey-800 text-center font-xssss fw-600 ls-1 rounded-xl text-decoration-none">Delete</a>
                                                              </div>
                                                            </div>`;
                                            content += request;
                                            break;
                                        }
                                    }
                                }
                            }

                            document.getElementById("friendRequestNotPanel").innerHTML = content;
                        },
                        error: function (err) { }
                    });
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }, 1000);
}

GetNotification();