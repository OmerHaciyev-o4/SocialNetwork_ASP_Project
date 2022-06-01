function SideBarOpeningFunc() {
    console.log("dsfsdfsadfasf");
    var classList = document.getElementById("renderPanel").classList;
    var st = false;

    for (var i = 0; i < classList.length; i++) {
        if (classList[i] == "right-chat-active") {
            st = true;
            break;
        }
    }

    console.log(st);
    if (st) {
        console.log("remove");
        document.getElementById("renderPanel").classList.remove('right-chat-active');
        document.getElementById("main-content-wrap").classList.remove('active-sidebar');
    }
    else if (!st) {
        console.log("add");
        document.getElementById("renderPanel").classList.add('right-chat-active');
        document.getElementById("main-content-wrap").classList.add('active-sidebar');
    }
}

function Follow(el, id) {
    el.innerHTML = "Sent";
    el.style.backgroundColor = "#79db91";

    $.ajax({
        url: "/Database/GetCurrentUser",
        method: "GET",
        success: function(user) {
            var notObj = {
                "Title": "Friend Request",
                "ReceiveUserId": Number(id),
                "SenderUserId": Number(user.id)
            }

            $.ajax({
                url: `/Database/AddNotification?notificationInJson=${JSON.stringify(notObj)}`,
                method: "POST",
                success: function () {
                    var alert = document.getElementById("searchResultAlert");
                    alert.classList.remove("d-none");
                    alert.classList.remove("alert-danger");
                    alert.classList.add("alert-success");
                    alert.innerHTML = "Your request has been successfully sent.";
                },
                error: function (err) {
                    var alert = document.getElementById("searchResultAlert");
                    alert.classList.remove("d-none");
                    alert.classList.remove("alert-success");
                    alert.classList.add("alert-danger");
                    alert.innerHTML = "You sent the request in advance.";
                }
            });
        },
        error: function(err) {}
    });
}

function UnFollow(el, id) {
    $.ajax({
        url: "/Database/GetCurrentUser",
        method: "GET",
        success: function(user) {
            $.ajax({
                url: `/Database/UnFollow?userId=${user.id}&friendId=${id}`,
                method: "POST",
                success: function () {
                    var alert = document.getElementById("searchResultAlert");
                    alert.classList.remove("d-none");
                    alert.classList.remove("alert-danger");
                    alert.classList.add("alert-success");
                    alert.innerHTML = "You're not friends anymore.";

                    el.innerHTML = "FOLLOW";
                    el.style.backgroundColor = "#28a745";
                    el.onclick = function () {
                        Follow(el, id);
                    }
                },
                error: function (err) {
                    var alert = document.getElementById("searchResultAlert");
                    alert.classList.remove("d-none");
                    alert.classList.remove("alert-success");
                    alert.classList.add("alert-danger");
                    alert.innerHTML = "We encountered an unexpected error. Try again.";
                }
            });
        }
    });
}


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
//document.getElementById("chatIconDesk").onclick = function() {
//    console.log("dsfsdfsadfasf");
//    var classList = document.getElementById("renderPanel").classList;
//    var st = true;

//    for (var i = 0; i < classList.length; i++) {
//        if (classList[i] == "right-chat-active") {
//            st = false;
//            break;
//        }
//    }

//    console.log(st);
//    if (st) {
//        document.getElementById("renderPanel").classList.remove('right-chat-active');
//        document.getElementById("main-content-wrap").classList.remove('active-sidebar');
//    }
//    else if (!st) {
//        document.getElementById("renderPanel").classList.add('right-chat-active');
//        document.getElementById("main-content-wrap").classList.add('active-sidebar');
//    }
//}

document.getElementById('smallSearch').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        console.log("yesss");
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

document.getElementById('appMode').addEventListener('click', function (event) {
    $.ajax({
        url: "/Home/ChangeUserAppMode",
        type: "POST",
        success: function (res) {
            var iconEl = document.getElementById('appModeIcon');
            var body = document.getElementsByTagName('body')[0];
            if (res == true) {
                iconEl.remove("feather-moon");
                iconEl.add("feather-sun");
                body.classList.remove('theme-light');
                body.classList.remove('theme-dark');
            }
            else {
                iconEl.remove("feather-sun");
                iconEl.add("feather-moon");
                body.classList.remove('theme-dark');
                body.classList.remove('theme-light');
            }
        },
        error: function (err) { }
    });

    event.preventDefault();
});

function StartFunc() {
    setInterval(function () {
        $.ajax({
            url: `/Database/GetActiveUsers`,
            method: "GET",
            success: function (users) {
                var content = "";
                var activeFriends = [];
                for (var i = 0; i < users.length; i++) {
                    var userVM = users[i];
                            
                    var imgPath = userVM.user.imageUrl;
                    if (imgPath == null) { imgPath = `/images/defaultImage.png`; }

                    var userContent =
                        `<li class="bg-transparent list-group-item no-icon pe-0 ps-0 pt-2 pb-2 border-0 d-flex align-items-center">
                                <figure class="avatar float-left mb-0 me-2">
                                    <img src="${imgPath}" alt="${userVM.user.username}" class="w35">
                                </figure>
                                <a href="/Home/Chat?id=${userVM.user.id}" class="fw-700 mb-0 mt-0 text-decoration-none">
                                    <span class="font-xssss text-grey-600 d-block text-dark model-popup-chat pointer">@${userVM.user.username}</span>
                                </a>`;
                    if (userVM.isActive) {
                        userContent +=
                            `<span class="bg-success ms-auto btn-round-xss"></span>
                        </li>`;
                    }
                    else {
                        userContent +=
                            `<span class="bg-danger ms-auto btn-round-xss"></span>
                        </li>`;
                    }

                    content += userContent;
                }

                document.getElementById("activeFriends").innerHTML = content;
            },
            error: function (err) { }
        });
    }, 500);
}

StartFunc();