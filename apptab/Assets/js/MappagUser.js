$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    GetListUser();

    $(`[data-id="auth-list"]`).change(function (k, v) {
        let val = $(this).val();
        if (val == "0") {
            $("#Connex").prop("disabled", true);
            $("#MDP").prop("disabled", true);
            $("#Connex").val("");
            $("#MDP").val("");
        } else {

            $("#Connex").prop("disabled", false);
            $("#MDP").prop("disabled", false);
        }
    });

    $("#base-container").hide();
});

function GetListUser() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/GetAllPROJET',
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

            $(`[data-id="societe-list"]`).text("");
            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
            });
            $(`[data-id="societe-list"]`).append(code);

        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}

$(`[data-id="connex"]`).click(function () {
    let usr = $("#Connex").val();
    let psw = $("#MDP").val();
    let inst = $("#Instance").val();
    let auth = "0";

    $("#base-container").hide();

    if (!inst) {
        alert("Veuillez renseigner l'instance. ");
        return;
    }
    if ($(`[data-id="auth-list"]`).val() == "1") {
        if (!usr && !psw) {
            alert("Veuillez renseigner les champs. ");
            return;
        }
    }

    if ($(`[data-id="auth-list"]`).val() != null) {
        auth = $(`[data-id="auth-list"]`).val();
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("map.INSTANCE", inst);
    formData.append("map.CONNEXION", usr);
    formData.append("map.CONNEXPWD", psw);
    formData.append("map.AUTH", auth);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/GetNewInstance',
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
            }

            $(`[data-id="base-list"]`).text("");
            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v}">${v}</option>
                `;
            });
            $(`[data-id="base-list"]`).append(code);

            $("#base-container").show();
        },
    });
});

$(`[data-action="AddnewUser"]`).click(function () {
    let formData = new FormData();
    let auth = "0";

    if ($(`[data-id="auth-list"]`).val() != null) {
        auth = $(`[data-id="auth-list"]`).val();
    }

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("user.INSTANCE", $(`#Instance`).val());
    formData.append("user.AUTH", auth);
    formData.append("user.CONNEXION", $(`#Connex`).val());
    formData.append("user.CONNEXPWD", $(`#MDP`).val());
    formData.append("user.DBASE", $(`#DataBase`).val());
    formData.append("user.IDPROJET", $(`#IDProjet`).val());

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/SuperAdminMaPCreate',
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
                window.location = Origin + "/SuperAdmin/SuperAdminMaPList";
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
