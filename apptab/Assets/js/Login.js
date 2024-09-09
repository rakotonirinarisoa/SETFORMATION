const urlOrigin = window.location.href;

let User;
let Origin;

let clickedId;

let ListCodeJournal;
let ListCompteG;

let content;
let validate;

let listResult;
let listResultAnomalie;
let contentAnomalies;
let rdv;
let contentpaie;
let listResultpaie;
let reglementresult;

let listEtat;
let etaCode;

var baseName;

let table = undefined;

const loader = $('#loader');

function GetFileUrlOrgF() {
    let formData = new FormData();

    formData.append("suser.LOGIN","a");
    formData.append("suser.PWD", "a");
    formData.append("suser.ROLE", "a");
    formData.append("suser.IDSOCIETE", "a");

    $.ajax({
        type: "POST",
        url: urlOrigin + '/GetFileF/GetFile',
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
            //urlOrigin = Datas.data,
             
            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                return;
            }
            
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
$("#login_mdp, #login_user").on("keydown", (e) => {

    if (e.keyCode == 13) $("[login_connect]").click(); //login();
});

$(`[login_connect]`).click(() => {
    let formData = new FormData();

    formData.append("Users.LOGIN", $('#login_user').val());
    formData.append("Users.PWD", $('#login_mdp').val());

    $.ajax({
        type: "POST",
        url: urlOrigin + '/User/Login',
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
                return;
            }

            let user = {
                LOGIN: $('#login_user').val(),
                PWD: $('#login_mdp').val(),
                ROLE: Datas.Data.ROLE,
                IDPROJET: Datas.Data.IDPROJET,
                origin: urlOrigin
            }

            sessionStorage.setItem("user", JSON.stringify(user));

            window.location = urlOrigin + "/Home/TdbAccueil";
        },
        error: function () {
            alert("");
        }
    });
});
