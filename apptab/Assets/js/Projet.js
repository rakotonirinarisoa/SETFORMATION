$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetListSociete();
});

function GetListSociete() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Projects/GetAllProjects',
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
                    <tr data-project-id="${v.ID}" class="text-nowrap last-hover">
                        <td>${v.PROJET}</td>

                         <td class="elerfr">
                            <div onclick="updateProject('${v.ID}')"><i class="fa fa-pen-alt text-warning"></i></div>
                        </td>
                        <td class="elerfr">
                            <div onclick="deleteProject('${v.ID}')"><i class="fa fa-trash text-danger"></i></div>
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

$(`[data-action="AddnewSociete"]`).click(function () {
    let formData = new FormData();
    let newpwd = $(`#MDP`).val();
    let newpwdConf = $(`#MDPC`).val();
    if (newpwd != newpwdConf) {
        alert("Les mots de passe ne correspondent pas. ");
        return;
    }

    let soc = $("#Proj").val();
    let firtAdm = $("#Login").val();
    if (!soc) {
        alert("Veuillez renseigner le projet. ");
        return;
    }
    if (!firtAdm) {
        alert("Veuillez renseigner le login du premier ADMINISTRATEUR. ");
        return;
    }

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("societe.PROJET", $(`#Proj`).val());

    formData.append("user.LOGIN", $(`#Login`).val());
    formData.append("user.PWD", $(`#MDP`).val());

    $.ajax({
        type: "POST",
        url: Origin + '/Projects/AddProject',
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
                window.location = Origin + "/Projects/AllProjects";
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;

                return;
            }
        },
    });
});

function updateProject(id) {
    window.location = Origin + "/Projects/ProjectDetails?id=" + id;
}

function deleteProject(id) {
    if (!confirm("Etes-vous sûr de vouloir supprimer ce projet ?")) return;

    const formData = new FormData();

    formData.append("login", User.LOGIN);
    formData.append("password", User.PWD);
    formData.append("id", id);

    $.ajax({
        type: 'POST',
        url: Origin + '/Projects/DeleteProject',
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

            alert(Datas.msg);
            $(`[data-project-id="${id}"]`).remove();
        }
    });
}
