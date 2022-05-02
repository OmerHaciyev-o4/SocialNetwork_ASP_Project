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


function searchResult(e) {
    if (e.keyCode === 13) {
        console.log(value);
        var objData = {
            "searchData": value
        };

        $.ajax({
            url: '/Home/SearchResult',
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify(objData),
            dataType: 'json',
        });
    }
}

var currentSearchData = "";

document.getElementById('smallSearch').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        console.log(this.value);
        var objData = {
            "searchData": this.value
        };

        $.ajax({
            url: '/Home/SearchResult',
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify(objData),
            dataType: 'json',
        });
    }
});
document.getElementById('searchPanel').addEventListener('keydown', function (e) {
    if (e.keyCode === 13) {
        console.log(this.value);
        var objData = {
            SearchData: this.value.toString()
        };

        //Show_Data();

        $.ajax({
            url: '/home/searchresult?data=' + JSON.stringify(objData),
            type: 'POST',
            error: function (error) {
                console.log(error);
            }
        });
    }
});



















//var conn = new ActiveXObject("ADODB.Connection"); //creating the connection object
//var conn_str = "";
//var db_Host = "";
//var db_User = "";
//var db_Password = "";
//var db_Provider = "";
//var db_Default = "";

////Data Source=DESKTOP-GLFPTE3\MSSQLSERVER01;Initial Catalog=SocialDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
//function Show_Data() {
//    db_Host = "DESKTOP-GLFPTE3"; //your computer name
//    db_User = "omeradmin"; //system admin user
//    db_Password = "123456789";
//    db_Provider = "MSSQLSERVER01";
//    db_Default = "SocialDB"; //database name
//    conn_str = "Provider=" + db_Provider + ";DataSource=" + db_Host + " ; User Id=" + db_User + " ;  password=" + db_Password + "; Initial Catalog=" + db_Default;
//    show_data_from_database();
//}

//function show_data_from_database() {
//    try {
//        conn.Open(conn_str); //open the connection
//        //alert(conn)
//        var reader = new ActiveXObject("ADODB.Recordset"); //creating an object of adodb to read the data as rows 
//        var strQuery = "SELECT * FROM  Users"; //query string 
//        reader.Open(strQuery, conn); //fetch the data
//        reader.MoveFirst(); //move to the first row
//        while (!reader.eof) //reaad until the last row of data
//        {
//            console.log(reader);
//            // document.write(reader.fields(0) + "&nbsp;&nbsp;&nbsp;"); //print to the screen
//            // document.write(reader.fields(1) + "&nbsp;&nb sp;&nbsp;");
//            // document.write(reader.fields(2) + "<br/>");
//            //alert(rs.fields(0));
//            reader.movenext(); //move to the next row
//        }
//    } catch (e) { }
//}


//var openedNot = false;


//class STVariable {
//    static isDark = false;
//}

//document.body.reload = function() {
//    if (STVariable.isDark) {
//        console.log(STVariable.isDark);

//        document.getElementById("appModeIcon").classList.add("clicked");
//        document.body.classList.add("theme-dark");
//        STVariable.isDark = true;
//    }
//    else {
//        console.log(STVariable.isDark);

//        document.getElementById("appModeIcon").classList.remove("clicked");
//        document.body.classList.remove("theme-dark");
//        STVariable.isDark = false;
//    }
//}

//document.getElementById("dropdownMenu3").onclick = function () {
//    if (!openedNot) {
//        document.getElementById("dropdownMenu3").classList.add("show");
//        document.getElementById("notificationID").classList.add("show");
//        openedNot = true;
//    }
//    else {
//        document.getElementById("dropdownMenu3").classList.remove("show");
//        document.getElementById("notificationID").classList.remove("show");
//        openedNot = false;
//    }
//}

//document.getElementById("appMode").onclick = function () {
//    if (!STVariable.isDark) {
//        console.log(STVariable.isDark + "   1");

//        document.getElementById("appModeIcon").classList.add("clicked");
//        document.body.classList.add("theme-dark");
//        STVariable.isDark = true;
//    }
//    else {
//        console.log(STVariable.isDark + "   2");

//        document.getElementById("appModeIcon").classList.remove("clicked");
//        document.body.classList.remove("theme-dark");
//        STVariable.isDark = false;
//    }
//}

//document.getElementById('searchPanel').addEventListener("keydown", function (e) {
//    if (e.keyCode === 13) {
//        $.ajax({
//            url: '/home/searchresult?data=' + document.getElementById('searchPanel').value,
//            type: 'POST'
//        });
//        document.getElementById('searchPanel').value = '';
//    }
//});