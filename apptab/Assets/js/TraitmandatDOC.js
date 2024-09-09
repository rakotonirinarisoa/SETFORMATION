$(document).ready(() => {
    $(`[data-widget="pushmenu"]`).on('click', () => {
        $(`[data-action="SaveSIIG"]`).toggleClass('custom-fixed-btn');
    });
});

let clickedIdD;

function modalD(id) {

    clickedIdD = id;

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IdF", clickedIdD);
    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/Traitement/ModalD',
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
            if (Datas.type == "success") {
                //window.location = window.location.origin;
                ListResult = Datas.data
                contentpaie = ``;
                $.each(ListResult, function (k, v) {
                    contentpaie += `
                    <tr class="select-text">
                        <td style="font-weight: bold; text-align:center">${v.REF}</td>
                        <td style="font-weight: bold; text-align:center">${v.OBJ}</td>
                        <td style="font-weight: bold; text-align:center">${v.TITUL}</td>
                        <td style="font-weight: bold; text-align:center">${formatCurrency(String(v.MONT).replace(",", "."))}</td>
                    </tr>
                    `                });
                $('.LIQUIMODAL').empty();
                $('.LIQUIMODAL').html(contentpaie);
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });

    $('#imputation-modal').modal('toggle');
}
