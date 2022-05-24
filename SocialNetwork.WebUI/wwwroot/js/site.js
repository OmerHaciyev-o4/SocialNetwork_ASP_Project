window.addEventListener('resize', function (e) {
    window.innerWidth > 1600
        ? document.getElementById('main-content-wrap').classList.add('active-sidebar')
        : document.getElementById('main-content-wrap').classList.remove('active-sidebar');
});

document.getElementById('noDesktopSearch').onclick = function () {
    document.getElementById('smallSearchPanel').classList.add('show');
}
document.getElementById('smallSearchCloseBtn').onclick = function() {
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

        setTimeout(function() {
            window.location = "/home/searchresult";
        }, 800);
    }
});

function GetNotification() {
    setInterval(function () {
        $.ajax({
            url: "/Database/GetNotification",
            method: "GET",
            success: function (data) {
                console.log(data);
                var friendNotifications = [];
                var currentNotificationLength = document.getElementById("friendRequestNotPanel").children.length;

                for (var i = 0; i < data.length; i++) {
                    if (data[i].title === "Friend Request") {
                        friendNotifications.push(data[i]);
                    }
                }

                if (currentNotificationLength != friendNotifications.length) {
                    var content = '';

                    for (var i = 0; i < friendNotifications.length; i++) {
                        var request = `<div class="wrap">
                                          <div class="card-body d-flex pt-0 ps-4 pe-4 pb-0 bor-0">
                                              <figure class="avatar me-3">
                                                  <img src="assets/images/user-7.png" alt="avater" class="shadow-sm rounded-circle w45"/>
                                              </figure>
                                              <h4 class="fw-700 text-grey-900 font-xssss mt-1">Anthony Daugloi
                                                  <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">12 mutual friends</span>
                                              </h4>
                                          </div>
                                          <div class="card-body d-flex align-items-center pt-0 ps-4 pe-4 pb-4">
                                              <a href="/defaultmember" class="p-2 lh-20 w100 bg-primary-gradiant me-2 text-white text-center font-xssss fw-600 ls-1 rounded-xl">Confirm</a>
                                              <a href="/defaultmember" class="p-2 lh-20 w100 bg-grey text-grey-800 text-center font-xssss fw-600 ls-1 rounded-xl">Delete</a>
                                          </div>
                                        </div>`;
                    }


                    
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    }, 1000);
}
function AddFollow(el, id) {
    var notObj = {
        "Title" : "Friend Request",
        "ReceiveUserId": Number(id)
    }


    $.ajax({
        url: `/Database/AddNotification?notificationInJson=${JSON.stringify(notObj)}`,
        method: "POST"
    });
}