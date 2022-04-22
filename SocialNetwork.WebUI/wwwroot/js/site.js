var openedNot = false;


class STVariable {
    static isDark = false;
}

document.body.reload = function() {
    if (STVariable.isDark) {
        console.log(STVariable.isDark);

        document.getElementById("appModeIcon").classList.add("clicked");
        document.body.classList.add("theme-dark");
        STVariable.isDark = true;
    }
    else {
        console.log(STVariable.isDark);

        document.getElementById("appModeIcon").classList.remove("clicked");
        document.body.classList.remove("theme-dark");
        STVariable.isDark = false;
    }
}

document.getElementById("dropdownMenu3").onclick = function () {
    if (!openedNot) {
        document.getElementById("dropdownMenu3").classList.add("show");
        document.getElementById("notificationID").classList.add("show");
        openedNot = true;
    }
    else {
        document.getElementById("dropdownMenu3").classList.remove("show");
        document.getElementById("notificationID").classList.remove("show");
        openedNot = false;
    }
}

document.getElementById("appMode").onclick = function () {
    if (!STVariable.isDark) {
        console.log(STVariable.isDark + "   1");

        document.getElementById("appModeIcon").classList.add("clicked");
        document.body.classList.add("theme-dark");
        STVariable.isDark = true;
    }
    else {
        console.log(STVariable.isDark + "   2");

        document.getElementById("appModeIcon").classList.remove("clicked");
        document.body.classList.remove("theme-dark");
        STVariable.isDark = false;
    }
}

document.getElementById('searchPanel').addEventListener("keydown", function (e) {
    if (e.keyCode === 13) {
        $.ajax({
            url: '/home/searchresult?data=' + document.getElementById('searchPanel').value,
            type: 'POST'
        });
        document.getElementById('searchPanel').value = '';
    }
});