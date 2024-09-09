$(document).ready(() => {
    $(`[data-widget="pushmenu"]`).on('click', () => {
        $(`[data-action="SaveSIIG"]`).toggleClass('custom-fixed-btn');
    });
});

let clickedANN;

function modalREJET(id) {

    clickedANN = id;

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IdF", clickedANN);
    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/TraitementAvance/GetIsMotif',
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

            $(`[data-id="MOTIF-list"]`).text("");

            var code = ``;
            ListResult = Datas.data
            $.each(ListResult, function (k, v) {
                code += `
                    <option value="${v.REF}" id="${v.REF}">${v.REF}</option>
                `;
            });

            $(`[data-id="MOTIF-list"]`).append(code);
        },
        error: function () {
            alert("Probl�me de connexion. ");
        }
    });

    $("#Commentaire").val("");

    $('#annuler-modal').modal('toggle');
}

$(`[data-action="ANNULMANDAT"]`).click(function () {
    let user = $("#Motif").val();
    if (!user) {
        alert("Veuillez renseigner le motif du rejet avant l'annulation de l'avance. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IdF", clickedANN);
    formData.append("Comm", $(`#Commentaire`).val());
    formData.append("Motif", user);
    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/TraitementAvance/AnnulationMandat',
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
            if (Datas.type == "success") {
                alert(Datas.msg);

                $(`[compteG-id="${clickedANN}"]`).remove();

                $("#annuler-modal").modal("toggle");

                return;
            }
        },
    });
});
