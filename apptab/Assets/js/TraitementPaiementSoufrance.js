var table = undefined;

let list = [];

const NUMBER_OF_ROWS = 5;


function setSum(array, startIndex, endIndex) {
    let total = 0;
    let totalPREVU = 0;
    let totalDEPASS = 0;

    for (let i = startIndex; i <= endIndex; i += 1) {
        total += Number(array[i].dureeTraitement);
        totalPREVU += Number(array[i].dureePrevu);
        totalDEPASS += Number(array[i].depassement);
    }

    array[endIndex + 1].dureeTraitement = total;
    array[endIndex + 1].dureePrevu = totalPREVU;
    array[endIndex + 1].depassement = totalDEPASS;
}
function calculateDuration(array) {
    let pointer = 0;

    for (let i = 0; i < array.length; i += 1) {
        if (i % NUMBER_OF_ROWS === NUMBER_OF_ROWS - 1) {
            setSum(array, pointer, i - 1);

            pointer = i + 1;
        }
    }
}

function parseList(array) {
    const result = [];

    let rowNumber = 0;
    console.log(array);
    for (let i = 0; i < array.length; i += 1) {
        rowNumber = 0;

        for (let j = 0; j < array[i].TraitementPaiementDetails.length; j += 1) {
            for (let k = 0; k < NUMBER_OF_ROWS; k += 1) {
                let etape = '';
                let beneficiaire = '';
                let dateTraitement = '';
                let montant = '';
                let agent = '';
                let dureeTraitement = '';
                let dureePrevu = '';
                let depassement = '';

                switch (k) {
                    case 0:
                        etape = 'Transfert et Validation OP';
                        dateTraitement = array[i].TraitementPaiementDetails[j].DATETRANSFERTRAF === undefined ? '' : formatDate(array[i].TraitementPaiementDetails[j].DATETRANSFERTRAF);
                        beneficiaire = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].BENEFICIAIRE;
                        montant = dateTraitement === '' ? '' : formatCurrency(String(array[i].TraitementPaiementDetails[j].MONTENGAGEMENT).replace(',', '.'));
                        agent = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].TRANSFERTRAFAGENT;
                        dureeTraitement = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTTRANSFERTOP;
                        dureePrevu = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTPREVUEOP;
                        depassement = array[i].TraitementPaiementDetails[j].DEPASSEMENTOP;

                        break;
                    case 1:
                        etape = 'Envoi AGENT COMPTABLE';
                        dateTraitement = array[i].TraitementPaiementDetails[j].DATEVALORDSEC === undefined ? '' : formatDate(array[i].TraitementPaiementDetails[j].DATEVALORDSEC);
                        beneficiaire = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].BENEFICIAIRE;
                        montant = dateTraitement === '' ? '' : formatCurrency(String(array[i].TraitementPaiementDetails[j].MONTENGAGEMENT).replace(',', '.'));
                        agent = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].VALORDSECAGENT;
                        dureeTraitement = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTTRANSFERTAC;
                        dureePrevu = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTPREVUEAC;
                        depassement = array[i].TraitementPaiementDetails[j].DEPASSEMENTAC;

                        break;
                    case 2:
                        etape = 'Validation AGENT COMPTABLE';
                        dateTraitement = array[i].TraitementPaiementDetails[j].DATESENDSIIG === undefined ? '' : formatDate(array[i].TraitementPaiementDetails[j].DATESENDSIIG);
                        beneficiaire = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].BENEFICIAIRE;
                        montant = dateTraitement === '' ? '' : formatCurrency(String(array[i].TraitementPaiementDetails[j].MONTENGAGEMENT).replace(',', '.'));
                        agent = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].SENDSIIGAGENT;
                        dureeTraitement = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTTRANSFERTBK;
                        dureePrevu = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTPREVUEBK;
                        depassement = array[i].TraitementPaiementDetails[j].DEPASSEMENTBK;

                        break;
                    case 3:
                        etape = 'Envoi Fichier BANQUE';
                        dateTraitement = array[i].TraitementPaiementDetails[j].DATESIIGFP === undefined ? '' : formatDate(array[i].TraitementPaiementDetails[j].DATESIIGFP);
                        beneficiaire = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].BENEFICIAIRE;
                        montant = dateTraitement === '' ? '' : formatCurrency(String(array[i].TraitementPaiementDetails[j].MONTENGAGEMENT).replace(',', '.'));
                        agent = dateTraitement === '' ? '' : array[i].TraitementPaiementDetails[j].SIIGFPAGENT;
                        dureeTraitement = '';
                        dureePrevu = array[i].TraitementPaiementDetails[j].DUREETRAITEMENTPREVUEBK;
                        depassement = '';

                        break;
                    default:
                        break;
                }

                result.push({
                    rowNumber,
                    soa: array[i].SOA,
                    num: array[i].TraitementPaiementDetails[j].NUM_ENGAGEMENT,
                    etape,
                    beneficiaire,
                    montant,
                    agent,
                    dateTraitement,
                    dureeTraitement,
                    dureePrevu,
                    depassement,
                    type: array[i].TraitementPaiementDetails[j].AVANCE ? 'Avance' : 'Paiement'
                });

                rowNumber += 1;
            }
        }
    }

    calculateDuration(result);

    list = result;
}

function setDataTable() {
    if (table !== undefined) {
        table.destroy();
    }

    table = $('#dashboard').DataTable({
        data: list,
        columns: [
            {
                data: 'soa'
            },
            {
                data: 'num'
            },
            {
                data: 'etape'
            },
            {
                data: 'beneficiaire'
            },
            {
                data: 'montant'
            },
            {
                data: 'agent'
            },
            {
                data: 'dateTraitement'
            },
            {
                data: 'dureeTraitement'
            },
            {
                data: 'dureePrevu'
            },
            {
                data: 'depassement'
            },
            {
                data: 'type'
            },
        ],
        lengthChange: false,
        paging: false,
        ordering: false,
        info: false,
        colReorder: false,
        rowsGroup: [0, 1],
        order: [['desc']],
        createdRow: function (row, data, _) {
            if (data.rowNumber !== 0) {
                $('td:eq(0)', row).addClass('delete-td');
                $('td:eq(1)', row).addClass('delete-td');
            }

            if (data.rowNumber % NUMBER_OF_ROWS === NUMBER_OF_ROWS - 1) {
                $('td:eq(3)', row).attr('colspan', 4).css({ 'text-align': 'center' });
                $('td:eq(3)', row).text('Durée totale');

                /*$('td:eq(4)', row).text(data.dateTraitement);*/
                $('td:eq(4)', row).text(data.dureeTraitement);
                $('td:eq(5)', row).text(data.dureePrevu);
                $('td:eq(6)', row).text(data.depassement);

                $('td:eq(7)', row).text('').css({ 'display': 'none' });
                $('td:eq(8)', row).text('').css({ 'display': 'none' });
                $('td:eq(9)', row).text('').css({ 'display': 'none' });
            }
        }
    });
}

$('[data-action="GenereLISTE"]').click(async function () {
    const dd = $("#dateD").val();
    const df = $("#dateF").val();

    if (!dd || !df) {
        alert("Veuillez renseigner les dates afin de générer la liste.");

        return;
    }

    const pr = $("#proj").val();

    if (!pr) {
        alert("Veuillez sélectionner au moins un projet.");

        return;
    }

    const formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);

    formData.append("DateDebut", $('#dateD').val());
    formData.append("DateFin", $('#dateF').val());

    formData.append("listProjet", $("#proj").val());

    $.ajax({
        type: 'POST',
        url: Origin + `/BordTraitement/GenereTraitementPaiementSouffrance`,
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            const res = JSON.parse(result);

            const { type, msg } = res;

            if (type === 'error' || type === 'PEtat' || type === 'Prese') {
                alert(msg);

                return;
            }

            const { data } = res;

            parseList(data);

            setDataTable();
        },
        Error: function (_, e) {
            alert(e);
        }
    });
});

$(document).ready(async () => {
    User = JSON.parse(sessionStorage.getItem("user"));

    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;

    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    $(`[data-widget="pushmenu"]`).on('click', () => {
        $(`[data-action="SaveV"]`).toggleClass('custom-fixed-btn');
    });

    GetListProjet();
});

$('#export-excel-btn').on('click', () => {
    $(`td.delete-td`).remove();

    tableToExcel('dashboard', 'DELAIS DE TRAITEMENT ENGAGEMENTS', setDataTable);
});

$('#proj').on('change', () => {
    emptyTable();
});

function checkdel(id) {
    $('.Checkall').prop("checked", false);
}

function GetListProjet() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/BordTraitement/GetAllPROJET',
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
            $.each(Datas.data.List, function (k, v) {
                code += `
                    <option value="${v.ID}">${v.PROJET}</option>
                `;
            });

            $(`[data-id="proj-list"]`).append(code);

            $("#proj").val([...Datas.data.PROJET]).trigger('change');
        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    })
}

$('.Checkall').change(function () {
    if ($('.Checkall').prop("checked") == true) {

        $('[compteg-ischecked]').prop("checked", true);
    } else {
        $('[compteg-ischecked]').prop("checked", false);
    }
});

function emptyTable() {
    const data = [];

    if (table !== undefined) {
        table.destroy();
    }

    table = $('#dashboard').DataTable({
        data,
        paging: false,
        ordering: false,
        info: false,
        colReorder: false
    });
}