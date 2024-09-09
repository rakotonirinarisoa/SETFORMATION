$(document).ready(() => {

    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    $(`[tab="autre"]`).hide();

    GetHistoriques()
    //GetListCodeJournal();
    //GetListCompG();
});
function GetHistoriques() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.ID", User.ID);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetHistoriques',
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
            //alert(Datas.data)
            if (Datas.type == "error") {
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
            ListResult = Datas.data
            ListResultBr = Datas.databr
            content = ``;
            $.each(ListResult, function (k, v) {
                contentpaie += `
                    <tr compteG-id="${v.NUMENREG}">
                        <td>${v.DATEAFB}</td>
                        <td>${v.NUMENREG}</td>
                        <td>${v.DATE}</td>
                        <td>${v.GUICHET}</td>
                        <td>${v.CODE_J}</td>
                        <td>${v.BANQUE}</td>
                        <td>${v.LIBELLE}</td>
                        <td>${v.MONTANT}</td>
                        <td>NULL</td>
                        <td>${v.RIB}</td>
                        <td>${v.LOGIN}</td>
                    </tr>`

            });
            $.each(ListResultBr, function (k, v) {
                contentpaie += `
                    <tr compteG-id="${v.NUMENREG}">
                        <td>${v.DATEAFB}</td>
                        <td>${v.NUMENREG}</td>
                        <td>${v.DATE}</td>
                        <td>${v.GUICHET}</td>
                        <td>${v.CODE_J}</td>
                        <td>${v.BANQUE}</td>
                        <td>${v.LIBELLE}</td>
                        <td>${v.MONTANT}</td>
                        <td>NULL</td>
                        <td>${v.RIB}</td>
                        <td>${v.LOGIN}</td>
                    </tr>`

            });
            $.each(ListResult, function (k, v) {
                content += `
                    <tr compteG-id="${v.NUMENREG}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td>
                        <td>${v.DATEAFB}</td>
                        <td>${v.NUMENREG}</td>
                        <td>${v.DATE}</td>
                        <td>${v.GUICHET}</td>
                        <td>${v.CODE_J}</td>
                        <td>${v.BANQUE}</td>
                        <td>${v.LIBELLE}</td>
                        <td>${v.MONTANT}</td>
                        <td>NULL</td>
                        <td>${v.RIB}</td>
                        <td>${v.LOGIN}</td>
                    </tr>`

            });
            $.each(ListResultBr, function (k, v) {
                content += `
                    <tr compteG-id="${v.NUMENREG}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td>
                        <td>${v.DATEAFB}</td>
                        <td>${v.NUMENREG}</td>
                        <td>${v.DATE}</td>
                        <td>${v.GUICHET}</td>
                        <td>${v.CODE_J}</td>
                        <td>${v.BANQUE}</td>
                        <td>${v.LIBELLE}</td>
                        <td>${v.MONTANT}</td>
                        <td>NULL</td>
                        <td>${v.RIB}</td>
                        <td>${v.LOGIN}</td>
                    </tr>`

            });
            $('#historiques').html(contentpaie);
            $('#CancelReg').html(content);
            //window.location = '/Home/GetFile?file=' + Datas.data;

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

$('[data-action="GetElementChecked"]').click(function () {
    let CheckList = $(`[compteg-ischecked]:checked`).closest("tr");
    let list = [];
    $.each(CheckList, (k, v) => {
        list.push($(v).attr("compteG-id"));
    });

    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("listCompte", list);
    formData.append("baseName", baseName);
    formData.append("journal", $('#commercial').val());
    formData.append("etat", "");
    formData.append("devise", false);

    let listid = list.splice(',');
    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCancel',
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
            //alert(Datas.data)
            if (Datas.type == "error") {
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
            ListResult = Datas.data;
            content = ``;
            GetHistoriques()
            //window.location = '/Home/GetFile?file=' + Datas.data;

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
});
$(`[tab="autre"]`).hide();
var baseName = "1";
$(`[name="options"]`).on("change", (k, v) => {
    var baseId = $(k.target).attr("data-id");
    baseName = baseId;
    if (baseId == "1") {
        $(`[tab="paie"]`).show();
        $(`[tab="autre"]`).hide();
    } else {
        $(`[tab="autre"]`).show();
        $(`[tab="paie"]`).hide();
    }
});
