window.addEventListener('resize', function (e) {
    window.innerWidth > 1600
        ? document.getElementById('main-content-wrap').classList.add('active-sidebar')
        : document.getElementById('main-content-wrap').classList.remove('active-sidebar');
});

document.getElementById('noDesktopSearch').onclick = function () {
    document.getElementById('smallSearchPanel').classList.add('show');
}
document.getElementById('smallSearchCloseBtn').onclick = function () {
    document.getElementById('smallSearchPanel').classList.remove('show');
}

document.getElementById('leftSideBarBtn').onclick = function () {
    this.className.toString() === "nav-menu me-0 ms-2 active"
        ? this.classList.remove('active')
        : this.classList.add('active');

    var leftSideBar = document.getElementById('leftSideBar');
    leftSideBar.className === "navigation scroll-bar nav-active"
        ? leftSideBar.classList.remove('nav-active')
        : leftSideBar.classList.add('nav-active');
}


document.getElementById('smallSearch').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        var splitedData = document.getElementById('searchPanel').value.toString()
            .split(/[$&+,:;=?|'<>.\-^*()%!_\[\]\{\}\\\"/~\`\']/);
        var splitedDataRowFirst = splitedData[0].split(' ');

        var searcedDataObj = {};
        searcedDataObj.RateSigns = "";
        searcedDataObj.Hashs = "";

        for (var i = 0; i < splitedDataRowFirst.length; i++) {
            if (splitedDataRowFirst[i] != (null || "")) {
                var fisrtSymbol = splitedDataRowFirst[i][0];
                if (fisrtSymbol == '@') {
                    var tempSplit = splitedDataRowFirst[i].split('@');
                    searcedDataObj.RateSigns += `${tempSplit[1]},`;
                }
                else if (fisrtSymbol == '#') {
                    var tempSplit = splitedDataRowFirst[i].split('#');
                    searcedDataObj.Hashs += `${tempSplit[1]},`;
                }
            }
        }

        var searchedDataObjToJson = JSON.stringify(searcedDataObj);

        $.ajax({
            url: '/home/searchresult?searchedData=' + searchedDataObjToJson,
            type: 'POST',
            success: function (response) {
                console.log(response);
                if (response) {
                }
            }
        });

        setTimeout(function () {
            window.location = "/home/searchresult";
        }, 500);
    }
});
document.getElementById('searchPanel').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        var splitedData = document.getElementById('searchPanel').value.toString()
            .split(/[$&+,:;=?|'<>\-^*()%!_\[\]\{\}\\\"/~\`\']/);
        var splitedDataRowFirst = splitedData[0].split(' ');

        var searcedDataObj = {};
        searcedDataObj.RateSigns = "";
        searcedDataObj.Hashs = "";

        for (var i = 0; i < splitedDataRowFirst.length; i++) {
            if (splitedDataRowFirst[i] != (null || "")) {
                var fisrtSymbol = splitedDataRowFirst[i][0];
                if (fisrtSymbol == '@') {
                    var tempSplit = splitedDataRowFirst[i].split('@');
                    searcedDataObj.RateSigns += `${tempSplit[1]},`;
                }
                else if (fisrtSymbol == '#') {
                    var tempSplit = splitedDataRowFirst[i].split('#');
                    searcedDataObj.Hashs += `${tempSplit[1]},`;
                }
            }
        }

        var searchedDataObjToJson = JSON.stringify(searcedDataObj);

        $.ajax({
            url: '/home/searchresult?searchedData=' + searchedDataObjToJson,
            type: 'POST',
            success: function (response) {
                console.log(response);
                if (response) {
                }
            }
        });

        setTimeout(function () {
            window.location = "/home/searchresult";
        }, 800);
    }
});

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
        var currentNotificationLength = 0;
        try {
            currentNotificationLength = document.getElementById("friendRequestNotPanel").children.length;
        } catch (e) { return; }

        $.ajax({
            url: "/Database/GetFriends",
            method: "GET",
            success: function (users) {
                var confirmFriendPanelCount = document.getElementById("confirmFriendPanel").children.length;
                if (confirmFriendPanelCount != users.length) {
                    var usersLength = users.length;
                    if (users.length > 6) { usersLength = 5; }

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
                try {
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
                } catch (e) { }

                if (notifications.length > 0 && currentNotificationLength != notifications.length) {
                    $.ajax({
                        url: "/Database/GetUsers",
                        method: "GET",
                        success: function (users) {
                            var content = '';

                            var notLength = notifications.length;
                            if (notifications.length > 6) {
                                notLength = 5;
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
    }, 1);
}
function AddFollow(el, id) {
    var notObj = {
        "Title": "Friend Request",
        "ReceiveUserId": Number(id)
    }


    $.ajax({
        url: `/Database/AddNotification?notificationInJson=${JSON.stringify(notObj)}`,
        method: "POST"
    });
}