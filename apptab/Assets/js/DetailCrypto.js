$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetUsers();
});

function GetUsers() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    /*formData.append("UserId", getUrlParameter("UserId"));*/

    $.ajax({
        type: "POST",
        url: Origin + '/Crypto/DetailsCrypto',
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

            /*$("#MDPA").val(Datas.data.CRYPTPWD);*/
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

$(`[data-action="UpdateUser"]`).click(function () {
    let newpwd = $(`#MDPN`).val();
    let newpwdConf = $(`#MDPC`).val();
    if (newpwd != newpwdConf) {
        alert("Les mots de passe ne correspondent pas. ");
        return;
    }

    //let MDPA = $(`#MDPA`).val(); 
    //if (!MDPA) {
    //    alert("Veuillez remplir l'ancien mot de passe");
    //    return;
    //}

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("user.CRYPTPWD", $(`#MDPN`).val());
    /*formData.append("MDPA", $(`#MDPA`).val());*/

    $.ajax({
        type: "POST",
        url: Origin + '/Crypto/UpdateCRT',
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
                window.location = urlOrigin + "/Crypto/CryptoList";
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
