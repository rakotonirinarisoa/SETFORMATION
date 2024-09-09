$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetListInstance();


    $(`#Instance`).change(function (k, v) {
        GetDB($("#Instance").val(), false);
    });
});

function GetListInstance() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: urlOrigin + '/DBaseTOM/GetInstance',
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

            $(`#Instance`).text("");
            var code = ``;
            $.each(Datas.data.list, function (k, v) {
                code += `
                    <option value="${v.INSTANCE}">${v.INSTANCE}</option>
                `;
            });
            $(`#Instance`).append(code);

            if (Datas.data.actual == null)
                return;

            $(`#Instance`).val(Datas.data.actual.INSTANCE);

            $(`input[data-id="${Datas.data.actual.TYPE}"`).click()
        },
        error: function (e) {
            alert("Probl�me de connexion. ");
        }
    }).done(function (x) {
        GetDB($(`#Instance`).val(), true);
    }).done(function (x) {
        //$(`#DataBase`).val($(`#Instance`).val());
    });
}

function GetDB(id, test) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("instance", id);

    $.ajax({
        type: "POST",
        url: urlOrigin + '/DBaseTOM/GetBase',
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

            $(`#DataBase`).text("");
            var code = ``;
            $.each(Datas.data.db, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.DBASE}</option>
                `;
            });
            $(`#DataBase`).append(code);
            if (test)
                $(`#DataBase`).val(Datas.data.mydb);
        },
        error: function (e) {
            alert("Probl�me de connexion. ");
        }
    });
}

$("#save").click(() => {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("bdd", $(`#DataBase`).val());
    formData.append("type", $("input[type='radio']:checked").attr("data-id"));

    $.ajax({
        type: "POST",
        url: urlOrigin + '/DBaseTOM/Save',
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

            alert("Enregistrement avec succ�s");
            window.location.reload();
        },
        error: function (e) {
            alert("Probl�me de connexion.");
        }
    });
});
$(`[name="options"]`).on("change", (k, v) => {

    var baseId = $(k.target).attr("data-id");
    baseName = baseId;
    //alert(baseName);
    if (baseId == "1") {
        $(`[tab="paie"]`).show();
        $(`[tab="autre"]`).hide();
        //GetListCodeJournal();
    } else {
        $(`[tab="autre"]`).show();
        $(`[tab="paie"]`).hide();
        //GetListCodeJournal();
    }

});
