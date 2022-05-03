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
        var objData = {
            SearchData: this.value.toString()
        };

        $.ajax({
            url: '/home/searchresult?data=' + JSON.stringify(objData),
            type: 'GET',
            error: function (error) {
                console.log(error);
            },
            success: function () {
                alert('YEAH');
            }
        });

        window.location = "/home/searchresult";
    }
});
document.getElementById('searchPanel').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        var objData = {
            SearchData: this.value.toString()
        };

        $.ajax({
            url: '/home/searchresult?data=' + JSON.stringify(objData),
            type: 'GET',
            error: function (error) {
                console.log(error);
            },
             success: function () {
                alert('YEAH');
            }
        });

        window.location = "/home/searchresult";
    }
});