$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    GetListRole();
    GetListProjet();
});

function GetListRole() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/User/GetAllRole',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            var Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $(`[data-id="User-role-list"]`).text("");

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${k}" id="${k}">${v}</option>
                `;
            });

            $(`[data-id="User-role-list"]`).append(code);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function GetListProjet() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/User/GetAllPROJET',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            var Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $(`[data-id="proj-list"]`).text("");
            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
            });
            $(`[data-id="proj-list"]`).append(code);

            GetUsers();
        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}

function GetUsers() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("UserId", getUrlParameter("UserId"));

    $.ajax({
        type: "POST",
        url: Origin + '/User/DetailsUser',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            var Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $("#Login").val(Datas.data.LOGIN);

            $("#Role").val(`${Datas.data.ROLE}`);

            $("#PROJET").val([...Datas.data.PROJET]).trigger('change');
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

//function UpdateUser(id) {
//    let newpwd = $(`#MDP`).val();
//    let newpwdConf = $(`#MDPC`).val();
//    if (newpwd != newpwdConf) {
//        alert("Les mots de passe ne correspondent pas");
//        return;
//    }

//    let formData = new FormData();

//    formData.append("suser.LOGIN", User.LOGIN);
//    formData.append("suser.PWD", User.PWD);
//    formData.append("suser.ROLE", User.ROLE);
//    formData.append("suser.IDPROJET", User.IDPROJET);

//    formData.append("UserId", id);

//    formData.append("user.LOGIN", $(`#Login`).val());
//    formData.append("user.ROLE", $(`#Role`).val());
//    formData.append("user.PWD", newpwd);

//    $.ajax({
//        type: "POST",
//        url: urlOrigin + '/User/UpdateUser',
//        data: formData,
//        cache: false,
//        contentType: false,
//        processData: false,
// beforeSend: function () {
//     loader.removeClass('display-none');
// },
// complete: function () {
//     loader.addClass('display-none');
// },
//        success: function (result) {
//            var Datas = JSON.parse(result);

//            if (Datas.type == "error") {
//                alert(Datas.msg);
//                return;
//            }
//            if (Datas.type == "success") {
//                alert(Datas.msg);
//                window.location = urlOrigin + "/User/List";
//                /*window.history.back();*/
//                /*location.replace(document.referrer);*/
//            }
//            if (Datas.type == "login") {
//                alert(Datas.msg);
//                window.location = window.location.origin;
//            }
//        },
//        error: function () {
//            alert("Connexion Problems");
//        }
//    });
//}

$(`[data-action="UpdateUser"]`).click(function () {
    let newpwd = $(`#MDP`).val();
    let newpwdConf = $(`#MDPC`).val();
    if (newpwd != newpwdConf) {
        alert("Les mots de passe ne correspondent pas. ");
        return;
    }

    let pr = $("#PROJET").val();
    if (!pr) {
        alert("Veuillez sélectionner au moins un projet. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("user.LOGIN", $(`#Login`).val());
    formData.append("user.ROLE", $(`#Role`).val());
    formData.append("user.PWD", newpwd);

    formData.append("oldPassword", $('#old-password').val());
    formData.append("UserId", getUrlParameter("UserId"));

    formData.append("listProjet", $("#PROJET").val());

    $.ajax({
        type: "POST",
        url: Origin + '/User/UpdateUser',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            var Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);

                return;
            }
            if (Datas.type == "success") {
                alert(Datas.msg);
                window.location = Origin + "/User/List";
                /*window.history.back();*/
                /*location.replace(document.referrer);*/
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
            }
        },
    });
});
