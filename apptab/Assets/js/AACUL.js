$(document).ready(() => {

    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    GetCountTDB();
});

function GetCountTDB() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/GetCountTDB/GetCountTDB',
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
                ListResult = Datas.data

                $("#MandatT").html(ListResult.MandatT);
                $("#MandatV").html(ListResult.MandatV);
                $("#MandatA").html(ListResult.MandatA);
                $("#MandatTR").html(ListResult.MandatTR);

                $("#MandatTA").html(ListResult.MandatTA);
                $("#MandatVA").html(ListResult.MandatVA);
                $("#MandatAA").html(ListResult.MandatAA);
                $("#MandatTRA").html(ListResult.MandatTRA);

                $("#PaieR").html(ListResult.PaieR);
                $("#PaieT").html(ListResult.PaieT);
                $("#PaieV").html(ListResult.PaieV);
                $("#PaieF").html(ListResult.PaieF);
                $("#PaieA").html(ListResult.PaieA);
            }
        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}
