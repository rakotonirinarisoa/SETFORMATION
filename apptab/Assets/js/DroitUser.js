$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    GetListUser();

    $(`[data-id="Instance-list"]`).change(function (k, v) {
        GetDB($(this).val());
    });
});

function GetListUser() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Admin/GetAllUser',
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

            $(`[data-id="User-list"]`).text("");
            var code = ``;
            $.each(Datas.data.USER, function (k, v) {
                code += `
                    <option value="${v.ID}" id="${v.ID}">${v.LOGIN}</option>
                `;
            });
            $(`[data-id="User-list"]`).append(code);

            $(`[data-id="Instance-list"]`).text("");
            var code = ``;
            $.each(Datas.data.INSTANCE, function (k, v) {
                code += `
                    <option value="${v.INSTANCE}">${v.INSTANCE}</option>
                `;
            });
            $(`[data-id="Instance-list"]`).append(code);

        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    }).done(function () {
        let instanceID = $(`[data-id="Instance-list"]`).val();

        GetDB(instanceID);
    })//.done(function (/*x*/) {
    //    $.ajax({
    //        type: "POST",
    //        url: urlOrigin + '/Admin/GetAllUser',
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
    //            if (Datas.type == "login") {
    //                alert(Datas.msg);
    //                window.location = window.location.origin;
    //                return;
    //            }

    //            $(`[data-id="User-list"]`).text("");

    //            var code = ``;
    //            $.each(Datas.data, function (k, v) {
    //                code += `
    //                <option value="${v.ID}" id="${v.ID}">${v.LOGIN}</option>
    //            `;
    //            });

    //            $(`[data-id="User-list"]`).append(code);

    //        },
    //        error: function () {
    //            alert("Problème de connexion");
    //        }
    //    });
    //});
}

function GetDB(instanceID) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("instanceID", instanceID);

    $.ajax({
        type: "POST",
        url: Origin + '/Admin/GetDB',
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

            $(`[data-id="DB-list"]`).text("");

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.DBASE}</option>
                `;
            });

            $(`[data-id="DB-list"]`).append(code);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    })
}

$(`[data-action="AddnewMapp"]`).click(function () {
    let user = $("#User").val();
    if (!user) {
        alert("Veuillez renseigner l'utilisateur. ");
        return;
    }
    let inst = $("#Instance").val();
    if (!inst) {
        alert("Veuillez renseigner l'instance. ");
        return;
    }
    let db = $("#DataBase").val();
    if (!db) {
        alert("Veuillez renseigner la base de données. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("droit.IDUSER", $(`#User`).val());
    formData.append("droit.IDMAPPAGE", $(`#DataBase`).val());

    $.ajax({
        type: "POST",
        url: Origin + '/Admin/AdminMaPUserCreate',
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
                window.location = urlOrigin + "/Admin/AdminMaPUserList";
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
