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

        //setTimeout(function() {
        //        window.location = "/home/searchresult";
        //    }, 500);
    }
});

function getting() {
    setInterval(function () {
        $.ajax({
            url: "/Database/GetNotification",
            method: "GET",
            success: function (data) {
                console.log(data);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }, 1000);
}
function follow(el, id) {

}