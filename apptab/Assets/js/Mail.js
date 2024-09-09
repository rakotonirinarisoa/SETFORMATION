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
        url: Origin + '/SuperAdmin/DetailsMail',
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
                $("#ParaT").val("");
                $("#ParaV").val("");
                $("#ParaSiig").val("");
                $("#ParaREJET").val("");

                $("#ParaTA").val("");
                $("#ParaVA").val("");
                $("#ParaSiigA").val("");
                $("#ParaREJETA").val("");
                
                $("#ParaREJETPAIE").val("");
                $("#ParaPi").val("");
                $("#ParaPe").val("");
                $("#ParaPv").val("");
                $("#ParaPp").val("");
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $("#ParaT").val(Datas.data.MAILTE);
            $("#ParaV").val(Datas.data.MAILTV);
            $("#ParaSiig").val(Datas.data.MAILSIIG);
            $("#ParaREJET").val(Datas.data.MAILREJET);

            $("#ParaTA").val(Datas.data.MAILTEA);
            $("#ParaVA").val(Datas.data.MAILTVA);
            $("#ParaSiigA").val(Datas.data.MAILSIIGA);
            $("#ParaREJETA").val(Datas.data.MAILREJETA);

            $("#ParaREJETPAIE").val(Datas.data.MAILREJETPAIE);
            $("#ParaPi").val(Datas.data.MAILPI);
            $("#ParaPe").val(Datas.data.MAILPE);
            $("#ParaPv").val(Datas.data.MAILPV);
            $("#ParaPp").val(Datas.data.MAILPP);
            //$("#ParaPb").val(Datas.data.MAILPB);

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
    let ParaT = $("#ParaT").val();
    let ParaV = $("#ParaV").val();
    let ParaSiig = $("#ParaSiig").val();
    let ParaREJET = $("#ParaREJET").val();

    let ParaTA = $("#ParaTA").val();
    let ParaVA = $("#ParaVA").val();
    let ParaSiigA = $("#ParaSiigA").val();
    let ParaREJETA = $("#ParaREJETA").val();

    let ParaREJETPAIE = $("#ParaREJETPAIE").val();
    let ParaPi = $("#ParaPi").val();
    let ParaPe = $("#ParaPe").val();
    let ParaPv = $("#ParaPv").val();
    let ParaPp = $("#ParaPp").val();
    //let ParaPb = $("#ParaPb").val();
    if (!ParaT || !ParaV || !ParaSiig || !ParaREJET || !ParaPi || !ParaPe || !ParaPv || !ParaPp || !ParaREJETPAIE /*|| !ParaPb*/ || !ParaTA || !ParaVA || !ParaSiigA || !ParaREJETA) {
        alert("Veuillez renseigner les mails. ");
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

    formData.append("param.MAILTE", $(`#ParaT`).val());
    formData.append("param.MAILTV", $(`#ParaV`).val());
    formData.append("param.MAILSIIG", $(`#ParaSiig`).val());
    formData.append("param.MAILREJET", $(`#ParaREJET`).val());

    formData.append("param.MAILTEA", $(`#ParaTA`).val());
    formData.append("param.MAILTVA", $(`#ParaVA`).val());
    formData.append("param.MAILSIIGA", $(`#ParaSiigA`).val());
    formData.append("param.MAILREJETA", $(`#ParaREJETA`).val());

    formData.append("param.MAILREJETPAIE", $(`#ParaREJETPAIE`).val());
    formData.append("param.MAILPI", $(`#ParaPi`).val());
    formData.append("param.MAILPE", $(`#ParaPe`).val());
    formData.append("param.MAILPV", $(`#ParaPv`).val());
    formData.append("param.MAILPP", $(`#ParaPp`).val());
    //formData.append("param.MAILPB", $(`#ParaPb`).val());

    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/UpdateMail',
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
