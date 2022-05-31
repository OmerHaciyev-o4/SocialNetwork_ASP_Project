var uniqueIds = [];
var selectedDatas = [];
var CLOUDINARY_URL = "https://api.cloudinary.com/v1_1/social-network-web/upload";
var CLOUDINARY_UPLOAD_PRESET = 'nihp1wvs';

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
        console.log(data);
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

function getData(formData) { 
    return axios({
        url: CLOUDINARY_URL,
        method: "POST",
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: formData
    });
};

async function SharePost() {
    console.log("Geldi");
    var postData = document.getElementById("message").value;

    var postObj = {
        SharedUserId: 0,
        PostMessage: postData,
        DataList: []
    };
    var addedCount = 0;

    for (var i = 0; i < selectedDatas.length; i++) {
        var formData = new FormData();
        formData.append('file', selectedDatas[i]);
        formData.append('upload_preset', CLOUDINARY_UPLOAD_PRESET);
        var res = await getData(formData);
        var type = selectedDatas[i].type.split('/')[0];
        postObj.DataList.push(`${res.data.url}~${type}`);
        addedCount++;
    }

    if (addedCount == selectedDatas.length) {
        console.log(postObj);

        $.ajax({
            url: `/Database/NewPost`,
            type: 'POST',
            dataType: "json",
            data: postObj,
            success : function(response) {
                console.log("Yes");
            },
            error: function (err) {
                console.log("NO :(((((");
            }
        });

        document.getElementById("imgupload").value = "";
        document.getElementById("message").value = "";
        document.getElementById("selectedData").innerHTML = "";
    }
}

function kFormatter(num) {
    return Math.abs(num) > 999 ? Math.sign(num) * ((Math.abs(num) / 1000).toFixed(1)) + 'K' : Math.sign(num) * Math.abs(num)
}

function DeleteRating(postId, status) {
    $.ajax({
        type: 'POST',
        url: `/Database/UpdateRating?data=${postId}~${status}`,
        success: function (data) {
            $.ajax({
                url: "/Database/GetPosts",
                method: "GET",
                success: function (usersPosts) {
                    ImplementPosts(usersPosts, usersPosts.length);

                    document.getElementById(`${postId}LikeBtn`).onclick = function () {
                        AddRating(postId, "add");
                    };

                    console.log(document.getElementById(`${postId}LikeBtn`));
                },
                error: function (error) { }
            });
        },
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
}

function AddRating(postId, status) {
    $.ajax({
        type: 'POST',
        url: `/Database/UpdateRating?data=${postId}~${status}`,
        success: function (data) {
            $.ajax({
                url: "/Database/GetPosts",
                method: "GET",
                success: function (usersPosts) {
                    ImplementPosts(usersPosts, usersPosts.length);

                    document.getElementById(`${postId}LikeBtn`).onclick = function () {
                        DeleteRating(postId, "delete");
                    };
                    console.log(document.getElementById(`${postId}LikeBtn`));
                },
                error: function (error) { }
            });
        },
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
}

function ImplementPosts(usersPosts, postCount) {
    var content = "";
    for (var i = usersPosts.length - 1; i >= 0 ; i--) {
        var userVM = usersPosts[i];
        var imgPath = userVM.user.imageUrl;
        if (userVM.user.imageUrl == null) { imgPath = "/images/defaultImage.png"; }

        for (var j = 0; j < userVM.posts.length; j++) {
            var postVM = userVM.posts[j];
            var timeDifference = calcuteDate(new Date(postVM.post.datePost.toString()), new Date());

            var postViewCode =
                `<div class="card w-100 shadow-xss rounded-xxl border-0 p-4 pb-0 mb-3">
                    <div class="card-body p-0 d-flex">
                        <figure class="avatar me-3">
                            <img src="${imgPath}" alt="${userVM.user.username}" class="shadow-sm rounded-circle w45" />
                        </figure>
                        <a href="/Home/Profile?id=${userVM.user.id}" class="text-dark fw-bold open-font mt-1 text-decoration-none">${userVM.user.username}
                            <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">${timeDifference} ago</span>
                        </a>
                        <div class="ms-auto pointer">
                            <i class="ti-more-alt text-grey-900 btn-round-md bg-greylight font-xss"></i>
                        </div>
                    </div>
                    <div class="card-body p-0 me-lg-5">
                        <p class="fw-500 text-grey-500 lh-26 font-xssss w-100 mb-2">${postVM.post.message}</p>
                    </div>
                    <div class="card-body d-block p-0 mb-3">
                        <div class="row ps-2 pe-2">`;

            if (postVM.postImages.length == 1) {
                if (postVM.postImages[0].postImageType == "image") {
                    postViewCode += `
                        <div class="col-xs-12 col-sm-12 p-1" onclick="openImageSlide(${postVM.postImages[0].postId})">
                            <img src="${postVM.postImages[0].postImageURL}" class="rounded-3 w-100"/>
                        </div>
                    </div>`;
                }
                else if (postVM.postImages[0].postImageType == "video") {
                    postViewCode += `
                        <div class="col-xs-12 col-sm-12 p-1" onclick="openImageSlide(${postVM.postImages[0].postId})">
                            <video  class="vjs-tech rounded-xxxl" style="width: 100%; height: auto;" controls>
                                <source src="${postVM.postImages[0].postImageURL}">
                            </video>
                        </div>
                    </div>`;
                }
            }
            else if (postVM.postImages.length == 2) {
                for (var k = 0; k < postVM.postImages.length; k++) {
                    if (postVM.postImages[k].postImageType == "image") {
                        postViewCode += `
                                    <div class="col-xs-6 col-sm-6 p-1" onclick="openImageSlide(${postVM.postImages[k].postId})">
                                        <img src="${postVM.postImages[k].postImageURL}" class="rounded-3 w-100"/>
                                    </div>`;
                    }
                    else if(postVM.postImages[k].postImageType == "video") {
                        postViewCode += `
                            <div class="col-xs-12 col-sm-12 p-1" onclick="openImageSlide(${postVM.postImages[k].postId})">
                                <video  class="vjs-tech" style="width: 100%; height: auto;" controls>
                                    <source src="${postVM.postImages[k].postImageURL}">
                                </video>
                            </div>`;
                    }
                }
                postViewCode += `</div>`;
            }
            else if (postVM.postImages.length >= 3) {
                for (var k = 0; k < 2; k++) {
                    if (postVM.postImages[k].postImageType == "image") {
                        postViewCode += `
                                    <div class="col-xs-6 col-sm-6 p-1" onclick="openImageSlide(${postVM.postImages[k].postId})">
                                        <img src="${postVM.postImages[k].postImageURL}" class="rounded-3 w-100"/>
                                    </div>`;
                    }
                    else if (postVM.postImages[k].postImageType == "video") {
                        postViewCode += `
                            <div class="col-xs-12 col-sm-12 p-1" onclick="openImageSlide(${postVM.postImages[k].postId})">
                                <video  class="vjs-tech" style="width: 100%; height: auto;" controls>
                                    <source src="${postVM.postImages[k].postImageURL}">
                                </video>
                            </div>`;
                    }
                }

                if (postVM.postImages[2].postImageType == "image") {
                    postViewCode += `<div class="col-xs-4 col-sm-4 p-1">
                                        <a class="position-relative d-block" href="#ClodinaryHref" data-lightbox="roadtrip">
                                            <img src="${postVM.postImages[2]}" class="rounded-3 w-100" alt="post" />
                                            <span class="img-count font-sm text-white ls-3 fw-600">
                                                <b>+${Number(postVM.postImage.length - 2)}</b>
                                            </span>
                                        </a>
                                    </div>
                                </div>`;
                }
                else if (postVM.postImages[2].postImageType == "video") {
                    postViewCode += `<div class="col-xs-12 col-sm-12 p-1">
                                        <a class="position-relative d-block" href="#ClodinaryHref" data-lightbox="roadtrip">
                                            <video  class="vjs-tech" style="width: 100%; height: auto;">
                                                <source src="${postVM.postImages[2].postImageURL}">
                                            </video>
                                            <span class="img-count font-sm text-white ls-3 fw-600">
                                                <b>+${Number(postVM.postImages.length - 2)}</b>
                                            </span>
                                        </a>
                                    </div>
                                </div>`;
                }
                //postViewCode += `
                //    <div class="col-xs-4 col-sm-4 p-1">
                //        <img src="assets/images/t-10.jpg" class="rounded-3 w-100" alt="post" />
                //    </div>
                //    <div class="col-xs-4 col-sm-4 p-1">
                //        <img src="assets/images/t-10.jpg" class="rounded-3 w-100" alt="post" />
                //    </div>
                //    <div class="col-xs-4 col-sm-4 p-1">
                //        <a class="position-relative d-block" href="#ClodinaryHref" data-lightbox="roadtrip">
                //            <img src="assets/images/t-10.jpg" class="rounded-3 w-100" alt="post" />
                //            <span class="img-count font-sm text-white ls-3 fw-600">
                //                <b>+${Number(postVM.postImage.length - 2)}</b>
                //            </span>
                //        </a>
                //    </div>
                //</div>`;
            }

            var rating = postVM.post.rating.toString();
            if (postVM.post.rating >= 1000) {
                rating = kFormatter(postVM.post.rating);
            }

            postViewCode += `
                <div class="card-body d-flex p-0">
                    <div class="emoji-bttn pointer d-flex align-items-center fw-600 text-grey-900 text-dark lh-26 font-xssss me-2">
                        <i id="${postVM.post.id}LikeBtn" class="feather-heart text-white bg-red-gradiant me-2 btn-round-xs font-xss" onclick="AddRating('${postVM.post.id}', 'add')"></i>${rating} Like
                    </div>
                    <a class="d-flex align-items-center fw-600 text-grey-900 text-dark lh-26 font-xssss text-decoration-none">
                        <i class="feather-message-circle text-dark text-grey-900 btn-round-sm font-lg"></i>
                        <span class="d-none-xss">0 Comment</span>
                    </a>
                    <div class="pointer ms-auto d-flex align-items-center fw-600 text-grey-900 text-dark lh-26 font-xssss" id="dropdownMenu32" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="feather-share-2 text-grey-900 text-dark btn-round-sm font-lg"></i>
                        <span class="d-none-xs">Share</span>
                    </div>
                </div>
            </div>
        </div>`;

            content += postViewCode;
        }
    }

    document.getElementById("posts").innerHTML = content;
}

function ConfirmNotification(senderId, myId, notId) {
    $.ajax({
        type: 'GET',
        url: `/Database/AddFriend?senderId=${senderId}&myId=${myId}&notId=${notId}`
    });
}
function CancelNotification(notId) {
    $.ajax({
        type: 'POST',
        url: `/Database/RemoveNotification?notId=${notId}`
    });
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
                    return minute + "min";
                }
                return hour + "h";
            }
            else if (day == 1) {
                return "Yesterday";
            }
            return `${sendDate.getDay()}/${sendDate.getMonth()}/${sendDate.getYear()}`;
        }
        return `${sendDate.getDay()}/${sendDate.getMonth()}/${sendDate.getYear()}`;
    }
    return `${sendDate.getDay()}/${sendDate.getMonth()}/${sendDate.getYear()}`;
}

function GetNotification() {
    setInterval(function () {
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
                        <div class="card-body bg-transparent-card d-flex p-3 bg-greylight ms-3 me-3 rounded-3 mb-3" id="${
                            users[i].id}">
                            <figure class="avatar me-2 mb-0">
                                <img src="${imgPath}" alt="${users[i].username
                            }" class="shadow-sm rounded-circle w45" />
                            </figure>
                            <h4 class="fw-700 text-grey-900 font-xssss mt-2 text-decoration-none">@${users[i].username}
                                <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">NaN</span>
                            </h4>
                            <a href="/Home/Profile?id=${users[i].id
                            }" class="btn-round-sm bg-white ms-auto mt-2 text-decoration-none">
                                <span class="feather-chevron-right font-xss text-grey-900"></span>
                            </a>
                        </div>`;

                        content += user;
                    }

                    document.getElementById("confirmFriendPanel").innerHTML = content;
                }
            },
            error: function (err) { }
        });
    }, 1000);

    setInterval(function () {
        $.ajax({
            url: "/Database/GetNotification",
            method: "GET",
            success: function (notifications) {
                var currentNotificationLength = document.getElementById("friendRequestNotPanel").children.length;
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
                        url: "/Database/GetCurrentUser",
                        method: "GET",
                        success: function (user) {
                            var content = '';
                            var notLength = notifications.length;
                            if (notifications.length > 4) {
                                notLength = 3;
                            }
                            for (var i = 0; i < notifications.length; i++) {
                                if (notifications[i].receiveUserId == user.id && notifications[i].title == "Friend Request") {
                                    var imgPath = "";
                                    if (user.imageUrl == null) {
                                        imgPath = `/images/defaultImage.png`;
                                    } else {
                                        imgPath = user.imageUrl;
                                    }

                                    var sendTimestr = calcuteDate(new Date(notifications[i].sendDate.toString()), new Date());

                                    var request = `<div class="wrap">
                                                              <div class="card-body d-flex pt-0 ps-4 pe-4 pb-0 bor-0">
                                                                  <figure class="avatar me-3">
                                                                      <img src="${imgPath}" alt="${user.username}" class="shadow-sm rounded-circle w45"/>
                                                                  </figure>
                                                                  <h4 class="fw-700 text-grey-900 font-xssss mt-1">@${user.username}
                                                                      <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">${sendTimestr} before</span>
                                                                  </h4>
                                                              </div>
                                                              <div class="card-body d-flex align-items-center pt-0 ps-4 pe-4 pb-4">
                                                                  <a onclick="ConfirmNotification(${user.id}, ${notifications[i].receiveUserId}, ${notifications[i].id})" class="p-2 lh-20 w100 bg-primary-gradiant me-2 text-white text-center font-xssss fw-600 ls-1 rounded-xl text-decoration-none">Confirm</a>
                                                                  <a onclick="CancelNotification(${notifications[i].id}) class="p-2 lh-20 w100 bg-grey text-grey-800 text-center font-xssss fw-600 ls-1 rounded-xl text-decoration-none">Delete</a>
                                                              </div>
                                                            </div>`;
                                    content += request;
                                }
                            }
                            document.getElementById("friendRequestNotPanel").innerHTML = content;
                        },
                        error: function (err) { }
                    });
                }
            },
            error: function (err) { console.log(err); }
        });
    }, 1000);

    setInterval(function () {
        $.ajax({
            url: "/Database/GetPosts",
            method: "GET",
            success: function (usersPosts) {
                console.log(usersPosts);
                var postCount = 0;
                for (var i = 0; i < usersPosts.length; i++) {
                    postCount += usersPosts[i].posts.length;
                }
                var currentPostsCount = document.getElementById("posts").children.length;


                if (postCount != currentPostsCount) {
                    ImplementPosts(usersPosts, usersPosts.length);
                }
            },
            error: function (error) { }
        });
    }, 1000);
}

GetNotification();