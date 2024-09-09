$(document).ready(async () => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    await GetListProjet();
    await GetUsers(undefined);
    await GetListMANDATP();
});
async function GetUsers(id) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);

    if (!id) {
        formData.append("suser.IDPROJET", User.IDPROJET);
    } else {
        formData.append("suser.IDPROJET", id);
    }

    $.ajax({
        type: "POST",
        async: true,
        url: Origin + '/Etat/DetailsInfoPro',
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

            $("#proj").val(`${Datas.data.PROJ}`);
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

async function GetListProjet() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Etat/GetAllPROJET',
        async: true,
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
            $(`[data-id="proj-list"]`).append(code);

        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    });
}

$('#proj').on('change', async () => {
    const idPROJET = $('#proj').val();

    let formData = new FormData();
    //alert(baseName);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);

    formData.append("IdPROJET", idPROJET);

    $.ajax({
        type: "POST",
        async: true,
        url: Origin + '/Etat/EtatMandatChange',
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
                    <tr compteG-id="${v.No}" class="select-text">
                        <td style="font-weight: bold; text-align:center">${v.REF}</td>
                        <td style="font-weight: bold; text-align:center">${v.OBJ}</td>
                        <td style="font-weight: bold; text-align:center">${v.TITUL}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.COMPTE}</td>
                        <td style="font-weight: bold; text-align:center">${v.PCOP}</td>
                        <td style="font-weight: bold; text-align:center">${formatCurrency(String(v.MONT).replace(",", "."))}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEDEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATETEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEBE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.STAT}</td>
                    `
                    if (v.STAT == "Attente validation") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div onclick="deleteUser('${v.No}')"><i class="fa fa-times fa-lg text-danger"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Validée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check fa-lg text-info"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Annulée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-ban fa-lg text-warning"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Traitée SIIGFP") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check-double fa-lg text-success"></i></div>
                                        </td >`
                    }

                    contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                        <div onclick="modalF('${v.No}')"><i class="fa fa-tags fa-lg text-info"></i></div>
                                    </td>
                                    </tr>`
                });

                $('.traitementPROJET').empty();
                $('.traitementPROJET').html(contentpaie);

                //$('#tableRFR').DataTable(
                //    {
                //        dom: 'Bfrtip',
                //        buttons: ['colvis'],
                //        colReorder: true,
                //        responsive: true,
                //        retrieve: true,
                //        paging: false
                //    }
                //)

                new DataTable(`#tableRFR`, {
                    dom: 'Bfrtip',
                    buttons: ['colvis'],
                    colReorder: true,
                    responsive: true,
                    retrieve: true,
                    paging: false
                });
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
});

function renderTree() {
    const $table = $('table');
    const rows = $table.find('tr');

    function reverseHide(table, element) {
        const $element = $(element);
        const id = $element.data('id');
        const children = table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            children.each(function (_, e) {
                reverseHide(table, e);
            });

            children.hide();
        }
    }

    rows.each(function (_, row) {
        const $row = $(row);
        const level = $row.data('level');
        const id = $row.data('id');
        const $columnName = $row.find('td[data-column="name"]');
        const children = $table.find('tr[data-parent="' + id + '"]');

        if (children.length) {
            $columnName.prepend(`
                <img
                    class="chevron chevron-right"
                    src="${Origin}/Assets/icons/chevron-right.svg" 
                    alt="chevron-right" 
                    width="15"
                    height="15"
                />
            `);

            children.hide();

            const expander = $columnName.find(`img.chevron`);

            expander.on('click', function (e) {
                const $target = $(e.target);

                if ($target.hasClass('chevron-right')) {
                    $target
                        .removeClass('chevron-right')
                        .addClass('chevron-down');

                    $target.attr('src', `${Origin}/Assets/icons/chevron-down.svg`)

                    children.show();

                    children.css({ backgroundColor: `${$row.css('backgroundColor')}` });
                } else {
                    $target
                        .removeClass('chevron-down')
                        .addClass('chevron-right');

                    $target.attr('src', `${Origin}/Assets/icons/chevron-right.svg`)

                    reverseHide($table, $row);
                }
            });
        }

        $columnName.prepend('<span class="treegrid-indent" style="width:' + 15 * level + 'px"></span>');
    });
};

//GET LISTE MANDAT PROJET//
async function GetListMANDATP() {
    let formData = new FormData();
    //alert(baseName);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);

    const id = $('#proj').val();

    $.ajax({
        type: "POST",
        async: true,
        url: Origin + '/Etat/EtatMandatProjet',
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
                    <tr compteG-id="${v.No}" class="select-text" @*data-id="${v.No}" data-parent="0" data-level="1"*@>
                        <td style="font-weight: bold; text-align:center" data-column="name">${v.REF}</td>
                        <td style="font-weight: bold; text-align:center">${v.OBJ}</td>
                        <td style="font-weight: bold; text-align:center">${v.TITUL}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.COMPTE}</td>
                        <td style="font-weight: bold; text-align:center">${v.PCOP}</td>
                        <td style="font-weight: bold; text-align:center">${formatCurrency(String(v.MONT).replace(",", "."))}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEDEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATETEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEBE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.STAT}</td>
                        
                    `
                    if (v.STAT == "Attente validation") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div onclick="deleteUser('${v.No}')"><i class="fa fa-times fa-lg text-danger"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Validée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check fa-lg text-info"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Annulée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-ban fa-lg text-warning"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Traitée SIIGFP") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check-double fa-lg text-success"></i></div>
                                        </td >`;
                    }

                    contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                        <div onclick="modalF('${v.No}')"><i class="fa fa-tags fa-lg text-info"></i></div>
                                    </td>
                                    </tr>`

                    //for (let a = 0; a < 100; a += 1) {
                    //    for (let j = 0; j < v.M.length; j += 1) {
                    //        const m = v.M[j];

                    //        contentpaie += `
                    //        <tr class="select-text-child" data-id="${m.No}" data-parent="${v.No}" data-level="2">
                    //            <td style="font-weight: bold; text-align:center" data-column="name"></td>
                    //            <td style="font-weight: bold; text-align:center">${m.OBJ}</td>
                    //            <td style="font-weight: bold; text-align:center">${v.TITUL}</td>
                    //            <td style="font-weight: bold; text-align:center">${formatDate(v.DATE)}</td>
                    //            <td style="font-weight: bold; text-align:center">${m.COMPTE}</td>
                    //            <td style="font-weight: bold; text-align:center">${m.PCOP}</td>
                    //            <td style="font-weight: bold; text-align:center">${formatCurrency(String(m.MONT).replace(",", "."))}</td>
                    //            <td style="font-weight: bold; text-align:center">${formatDate(v.DATEDEF)}</td>
                    //            <td style="font-weight: bold; text-align:center">${formatDate(v.DATETEF)}</td>
                    //            <td style="font-weight: bold; text-align:center">${formatDate(v.DATEBE)}</td>
                    //            <td style="font-weight: bold; text-align:center"></td>
                    //            <td style="font-weight: bold; text-align:center"></td>
                    //            <td style="font-weight: bold; text-align:center"></td>
                    //        </tr>
                    //    `
                    //    }
                    //}

                });

                $('.traitementPROJET').empty();
                $('.traitementPROJET').html(contentpaie);

                //renderTree();

                new DataTable(`#tableRFR`, {
                    dom: 'Bfrtip',
                    buttons: ['colvis'],
                    colReorder: true,
                    responsive: true,
                    retrieve: true,
                    paging: false
                });
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

//FILTRE PROJET//
$('[data-action="SearchPROJET"]').click(async function () {
    let dd = $("#dateD").val();
    let df = $("#dateF").val();
    if (!dd || !df) {
        alert("Veuillez renseigner les dates afin de filtrer les mandats. ");
        return;
    }

    let formData = new FormData();
    //alert(baseName);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);

    formData.append("DateDebut", $('#dateD').val());
    formData.append("DateFin", $('#dateF').val());
    formData.append("STAT", $('#stat').val());

    const id = $('#proj').val();

    $.ajax({
        type: "POST",
        async: true,
        url: Origin + '/Etat/EtatMandatProjetSEARCH',
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
                    <tr compteG-id="${v.No}" class="select-text">
                        <td style="font-weight: bold; text-align:center">${v.REF}</td>
                        <td style="font-weight: bold; text-align:center">${v.OBJ}</td>
                        <td style="font-weight: bold; text-align:center">${v.TITUL}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.COMPTE}</td>
                        <td style="font-weight: bold; text-align:center">${v.PCOP}</td>
                        <td style="font-weight: bold; text-align:center">${formatCurrency(String(v.MONT).replace(",", "."))}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEDEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATETEF)}</td>
                        <td style="font-weight: bold; text-align:center">${formatDate(v.DATEBE)}</td>
                        <td style="font-weight: bold; text-align:center">${v.STAT}</td>
                    `
                    if (v.STAT == "Attente validation") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div onclick="deleteUser('${v.No}')"><i class="fa fa-times fa-lg text-danger"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Validée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check fa-lg text-info"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Annulée") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-ban fa-lg text-warning"></i></div>
                                        </td >`
                    }
                    else if (v.STAT == "Traitée SIIGFP") {
                        contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                            <div><i class="fa fa-check-double fa-lg text-success"></i></div>
                                        </td >`
                    }

                    contentpaie += `<td class="elerfr" style="font-weight: bold; text-align:center">
                                        <div onclick="modalF('${v.No}')"><i class="fa fa-tags fa-lg text-info"></i></div>
                                    </td>
                                    </tr>`
                });

                $('.traitementPROJET').empty();
                $('.traitementPROJET').html(contentpaie);

                new DataTable(`#tableRFR`, {
                    dom: 'Bfrtip',
                    buttons: ['colvis'],
                    colReorder: true,
                    responsive: true,
                    retrieve: true,
                    paging: false
                });
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
});

function deleteUser(id) {
    if (!confirm("Etes-vous sûr de vouloir annuler cette ligne ?")) return;
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    formData.append("UserId", id);

    $.ajax({
        type: "POST",
        url: Origin + '/Etat/DeleteUser',
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

            if (Datas.type == "success") {
                alert(Datas.msg);

                $('.traitementPROJET').empty();
                $('.traitementPROJET').html(contentpaie);

                GetListProjet();
                GetUsers(undefined);
                GetListMANDATP();

                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
            else {
                alert(Datas.msg);
                return;
            }
        },
        error: function () {
            alert("Connexion Problems");
        }
    });
}

var toggler = document.getElementsByClassName("caret");
var i;

for (i = 0; i < toggler.length; i++) {
    toggler[i].addEventListener("click", function () {
        this.parentElement.querySelector(".nested").classList.toggle("active");
        this.classList.toggle("caret-down");
    });
}
