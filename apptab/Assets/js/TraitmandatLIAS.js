$(document).ready(() => {
    $(`[data-widget="pushmenu"]`).on('click', () => {
        $(`[data-action="SaveSIIG"]`).toggleClass('custom-fixed-btn');
    });
});

let clickedIdDL;

function modalLIAS(id) {

    clickedIdDL = id;

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("IdF", clickedIdDL);
    formData.append("iProjet", $("#proj").val());

    $.ajax({
        type: "POST",
        url: Origin + '/Traitement/ModalLIAS',
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
                if (ListResult.REF != "" || ListResult.OBJ != "" || ListResult.TITUL != "") {
                    contentpaie += `<tr class="select-text">
                                        <td style="font-weight: bold; text-align:center">DEF</td>
                                        <td style="font-weight: bold; text-align:center"><a href="${ListResult.REF}" target="_blank">${ListResult.REF}</a></td>
                                    </tr>
                                    <tr class="select-text">
                                        <td style="font-weight: bold; text-align:center">TEF</td>
                                        <td style="font-weight: bold; text-align:center"><a href="${ListResult.OBJ}" target="_blank">${ListResult.OBJ}</a></td>
                                    </tr>
                                    <tr class="select-text">
                                        <td style="font-weight: bold; text-align:center">BE</td>
                                        <td style="font-weight: bold; text-align:center"><a href="${ListResult.TITUL}" target="_blank">${ListResult.TITUL}</a></td>
                                    </tr>`;
                }

                $('.DOCMODAL').empty();
                $('.DOCMODAL').html(contentpaie);
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });

    $('#document-modal').modal('toggle');
}
