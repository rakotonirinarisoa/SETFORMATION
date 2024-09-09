$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    //GetListSociete();
    GetListProjet(getUrlParameter("PROSOAID"));
    GetListFPROSOA(getUrlParameter("PROSOAID"));
});

function GetListProjet(id) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IDPROSOA", id);

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

            $(`[data-id="proj-list"]`).text("");
            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
            });
            $.each(Datas.datas, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
            });
            $(`[data-id="proj-list"]`).append(code);

        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}

function GetListFPROSOA(id) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IDPROSOA", id);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/GetAllSOA',
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

            $(`[data-id="soa-list"]`).text("");
            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.SOA}</option>
                `;
            });
            $.each(Datas.datas, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.SOA}</option>
                `;
            });
            $(`[data-id="soa-list"]`).append(code);

        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}

$(`[data-action="AddnewSociete"]`).click(function () {
    let formData = new FormData();

    let socP = $("#Proj").val();
    if (!socP) {
        alert("Veuillez renseigner le projet. ");
        return;
    }

    let soc = $("#Soa").val();
    if (!soc) {
        alert("Veuillez renseigner le SOA. ");
        return;
    }

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("societe.PROJET", $(`#Proj`).val());
    formData.append("societe.SOA", $(`#Soa`).val());

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/AddSocietePROSOA',
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
                window.location = urlOrigin + "/SuperAdmin/PROSOAList";
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
        },
    });
});

function GetListSociete() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/FillTablePROSOA',
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

            $(`[data-id="ubody"]`).text("");

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <tr data-PROJETId="${v.ID}" class="text-nowrap last-hover">
                        <td>${v.PROJET}</td>
                        <td>${v.SOA}</td>
                        <td class="elerfr">
                            <div onclick="DetailFPROSOA('${v.ID}')"><i class="fa fa-pen-alt text-warning"></i></div>
                        </td>
                        <td class="elerfr">
                            <div onclick="deletePROSOA('${v.ID}')"><i class="fa fa-trash text-danger"></i></div>
                        </td>
                    </tr>
                `;
            });

            $(`[data-id="ubody"]`).append(code);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
function DetailFPROSOA(id) {
    window.location = urlOrigin + "/SuperAdmin/SuperAdminDetailFPROSOA?PROSOAID=" + id;
}
function deletePROSOA(id) {
    //alert("eto");
    if (!confirm("Etes-vous sûr de vouloir supprimer le mappage du PROJET ET SOA ?")) return;
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("PROSOAID", id);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/DeleteFPROSOA',
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
            $(`[data-PROJETId="${id}"]`).remove();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
//UpdateFPROSOA
$(`[data-action="UpdateFPROSOA"]`).click(function () {
    let formData = new FormData();
    let idprosoauP = getUrlParameter("PROSOAID");
    let socP = $("#Proj").val();
    if (!socP) {
        alert("Veuillez renseigner le projet. ");
        return;
    }

    let soc = $("#Soa").val();
    if (!soc) {
        alert("Veuillez renseigner le SOA. ");
        return;
    }

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("societe.IDPROJET", $(`#Proj`).val());
    formData.append("societe.IDSOA", $(`#Soa`).val());
    formData.append("idprosoaUp", idprosoauP);

    $.ajax({
        type: "POST",
        url: Origin + '/SuperAdmin/UpdateFPROSOA',
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
                window.location = Origin + "/SuperAdmin/PROSOAList";
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
        },
    });
});
