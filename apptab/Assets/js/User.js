$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetListUser();
});

function GetListUser() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/User/FillTable',
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
                alert("eeee" + Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $(`[data-id="ubody"]`).text("");

            /* <td>${v.PWD}</td> */

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <tr data-userId="${v.ID}" class="text-nowrap last-hover">
                        <td>${v.PROJET}</td>
                        <td>${v.LOGIN}</td>
                        <td>${v.ROLE}</td>
                        
                        <td>
                            <img
                                src="/Assets/icons/eye.svg"
                                width="20" height="20"
                                alt="Show password"
                                style="cursor: pointer;"
                                onclick="showPassword('${v.ID}')"
                            />
                        </td>

                        <td class="elerfr">
                            <div onclick="DetailUpdateUser('${v.ID}')"><i class="fa fa-pen-alt text-warning"></i></div>
                        </td>
                        <td class="elerfr">
                            <div onclick="deleteUser('${v.ID}')"><i class="fa fa-trash text-danger"></i></div>
                        </td>
                    </tr >`;

                //if (v.ROLE != "SAdministrateur") {
                //    code += `<td class="elerfr">
                //                <div onclick="linkeo('${v.ID}')"><i class="fa fa-link fa-lg text-primary"></i></div>
                //            </td>
                //    </tr >
                //`;
                //}
                //else {
                //    code += `<td class="elerfr">
                //                <div><i></i></div>
                //            </td>
                //    </tr >
                //`;
                //}

            });

            $(`[data-id="ubody"]`).append(code);
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function deleteUser(id) {
    if (!confirm("Etes-vous sûr de vouloir supprimer l'utilisateur ?")) return;
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("UserId", id);

    $.ajax({
        type: "POST",
        url: Origin + '/User/DeleteUser',
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
                alert(Datas.msg);
                $(`[data-userId="${id}"]`).remove();
                return;
            }
        },
        error: function () {
            alert("Connexion Problems");
        }
    });
}

function DetailUpdateUser(id) {
    window.location = Origin + "/User/DetailsUser?UserId=" + id;
}
