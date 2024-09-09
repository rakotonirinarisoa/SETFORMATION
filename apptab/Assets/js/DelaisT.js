$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;
    $(`[data-id="username"]`).text(User.LOGIN);
    GetListProjet();
});

function GetUsers() {
    let formData = new FormData();

    formData.append("iProjet", $("#proj").val());

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/DetailsDelais',
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
                $("#ParaV").val("");
                $("#ParaS").val("");
                $("#ParaSiig").val("");
                $("#ParaPe").val("");
                $("#ParaPv").val("");
                $("#ParaPp").val("");
                $("#ParaPb").val("");
                $("#ParaRAF").val("");

                $("#ParaRAFA").val("");
                $("#ParaVA").val("");
                $("#ParaSiigA").val("");
                $("#ParaSA").val("");

                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $("#ParaV").val(Datas.data.DELTV);
            $("#ParaVA").val(Datas.data.DELAV);
            $("#ParaS").val(Datas.data.DELSIIGFP);
            $("#ParaSA").val(Datas.data.DELASIIGFP);
            $("#ParaSiig").val(Datas.data.DELENVOISIIGFP);
            $("#ParaSiigA").val(Datas.data.DELAENVOISIIGFP);
            $("#ParaPe").val(Datas.data.DELPE);
            $("#ParaPv").val(Datas.data.DELPV);
            $("#ParaPp").val(Datas.data.DELPP);
            $("#ParaPb").val(Datas.data.DELPB);
            $("#ParaRAF").val(Datas.data.DELRAF);
            $("#ParaRAFA").val(Datas.data.DELARAF);

            if (Datas.data.IDPROJET != 0)
                $("#proj").val(`${Datas.data.IDPROJET}`);
            else
                $("#proj").val("");
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

$('#proj').on('change', () => {
    const id = $('#proj').val();
    GetUsers(id);
});

$(`[data-action="UpdateUser"]`).click(function () {
    let ParaV = $("#ParaV").val();
    let ParaVA = $("#ParaVA").val();
    let ParaS = $("#ParaS").val();
    let ParaSA = $("#ParaSA").val();
    let ParaSiig = $("#ParaSiig").val();
    let ParaSiigA = $("#ParaSiigA").val();
    let ParaPe = $("#ParaPe").val();
    let ParaPv = $("#ParaPv").val();
    let ParaPp = $("#ParaPp").val();
    let ParaPb = $("#ParaPb").val();
    let ParaRAF = $("#ParaRAF").val();
    let ParaRAFA = $("#ParaRAFA").val();
    if (!ParaV || !ParaS || !ParaSiig || !ParaPe || !ParaPv || !ParaPp || !ParaPb || !ParaRAF || !ParaVA || !ParaSA || !ParaSiigA || !ParaRAFA) {
        alert("Veuillez renseigner les délais de traitement. ");
        return;
    }

    let pr = $("#proj").val();
    if (!pr) {
        alert("Veuillez sélectionner au moins un projet. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("param.DELTV", $(`#ParaV`).val());
    formData.append("param.DELAV", $(`#ParaVA`).val());
    formData.append("param.DELSIIGFP", $(`#ParaS`).val());
    formData.append("param.DELASIIGFP", $(`#ParaSA`).val());
    formData.append("param.DELENVOISIIGFP", $(`#ParaSiig`).val());
    formData.append("param.DELAENVOISIIGFP", $(`#ParaSiigA`).val());
    formData.append("param.DELPE", $(`#ParaPe`).val());
    formData.append("param.DELPV", $(`#ParaPv`).val());
    formData.append("param.DELPP", $(`#ParaPp`).val());
    formData.append("param.DELPB", $(`#ParaPb`).val());
    formData.append("param.DELRAF", $(`#ParaRAF`).val());
    formData.append("param.DELARAF", $(`#ParaRAFA`).val());

    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/UpdateDelais',
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
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
            }
        },
    });
});

function GetListProjet() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Parametre/GetAllPROJET',
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
            //let i = 0;
            let pr = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
                //pr = v.PROJET;
                //i++;
            });

            $(`[data-id="proj-list"]`).append(code);

            GetUsers();
        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}
