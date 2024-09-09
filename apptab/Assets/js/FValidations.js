var table = undefined;
var FilenameUsr;
function checkdel(id) {
    $('.Checkall').prop("checked", false);
}

function GetEtat() {
    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetEtat',
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
            listEtat = Datas.data
            if (Datas.type == "error") {
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);

                return;
            }
            etaCode = `<option value = "Tous" > Tous</option> `;
            $.each(listEtat, function (_, v) {
                etaCode += `
                    <option value="${v}">${v}</option>
                `;
            });
            $(`[ETAT-list]`).html('');
            $(`[ETAT-list]`).append(etaCode);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
function GetTypeP() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetTypeP',
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
            baseName = Datas;
            if (baseName == 1) {
                $(`[code_Type]`).val('');
                $(`[code_Type]`).val('BR');

            } else {
                $(`[code_Type]`).val('');
                $(`[code_Type]`).val('COMPTA');
            }

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
};
function GetListCompG() {
    let formData = new FormData();

    let codeproject = $("#Fproject").val();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", baseName);
    formData.append("codeproject", codeproject);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCompteG',
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
            let code = ``;
            let codeAuxi = ``;
            ListCompteG = Datas.data;

            $.each(ListCompteG, function (_, v) {
                code += `
                    <option value="${v.COGE}">${v.COGE}</option>
                `;
            });
            $(`[compG-list]`).html('');
            $(`[compG-list]`).append(code);

            FillAUXI();
            FillCompteName();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function FillAUXI() {
    var list = ListCompteG.filter(x => x.COGE == $(`[compG-list]`).val()).pop();
    let code = `<option value="Tous"> Tous</option> `;
    $.each(list.AUXI, function (_, v) {
        code += `
            <option value="${v}">${v}</option>
        `;
    });

    $(`[auxi-list]`).html('');
    $(`[auxi-list]`).html(code);
}

function FillCompteName() {
    var nom = $(`[compG-list]`).val() + " " + $(`[auxi-list]`).val();
    $(`[compte-name`).val(nom);
}

function GetListCodeJournal() {
    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    //alert(codeproject);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);
    formData.append("codeproject", codeproject);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCODEJournal',
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

            let code = ``;
            ListCodeJournal = Datas.data;

            $.each(ListCodeJournal, function (_, v) {
                code += `
                    <option value="${v.CODE}">${v.CODE}</option>
                `;
            });

            $(`[codej-list]`).append(code);
            $(`[codej-libelle]`).val(ListCodeJournal[0].LIBELLE);
            GetEtat();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    }).done(function (res) {
        GetListCompG();
    });
}
//==============================================================================================Get text===================================================================================
function GetFileNameAnarana(blobUrl) {
    $.ajax({
        type: "POST",
        url: Origin + '/Home/FileName',
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
            console.log(result);
            const obj = JSON.parse(result)
            FilenameUsr = obj.Filename

            let a = document.createElement("a");
            a.href = blobUrl;
            a.download = FilenameUsr+".txt";
            document.body.appendChild(a);
            a.click();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
function getelementTXT(a) {
    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);
    alert(a);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.ID", User.ID);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", baseName);
    formData.append("codeJ", $('#commercial').val());
    formData.append("devise", false);
    formData.append("intbasetype", a);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/CreateZipFile',
        data: formData,
        cache: false,
        contentType: false,
        datatype: 'json',
        xhrFields: {
            responseType: 'blob'
        },
        processData: false,
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function (result) {
            console.log(result);
            let blobUrl = URL.createObjectURL(result);
            GetFileNameAnarana(blobUrl);


            //window.location = '/Home/GetFile?file=' + Datas.data;

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
//==============================================================================================Get All Project===================================================================================

function GetAllProjectUser() {

    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("codeproject", codeproject);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetAllProjectUser',
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

            reglementresult = ``;

            reglementresult = Datas.data;

            let listproject = ``;

            if (reglementresult.length) {
                $.each(reglementresult, function (k, v) {
                    listproject += `<option value="${v.ID}">${v.PROJET}</option>`;
                })
            } else {
                listproject += `<option value="${reglementresult.ID}" selected>${reglementresult.PROJET}</option>`;
            }

            $("#Fproject").html(listproject);
            GetTypeP();
            GetListCodeJournal();
            LoadValidate();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
//==============================================================================================Load Page===================================================================================

function LoadValidate() {

    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("suser.IDPROJET", User.IDPROJET);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/LoadValidateEcriture',
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
            reglementresult = ``;
            validate = ``;
            reglementresult = Datas.data;
            $('#afb').html('');

            const data = [];

            $.each(reglementresult, function (_, v) {
                data.push({
                    id: v.IDREGLEMENT,
                    dateOrdre: v.dateOrdre === null ? '' : v.dateOrdre,
                    noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                    compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                    libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                    debit: isNullOrUndefined(v.Debit) ? '' : v.Debit,
                    credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                    montant: isNullOrUndefined(v.MONTANT) ? '' : formatCurrency(String(v.MONTANT).replace(",", ".")),
                    montantDevise: v.MontantDevise === 0 ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                    mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                    rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                    financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                    commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                    plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                    journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                    marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                    isLATE: v.isLATE
                });
            });
            if (table !== undefined) {
                table.destroy();
            }
            table = $('#TDB').DataTable({
                data,
                columns: [
                    { data: 'id' },
                    { data: 'dateOrdre' },
                    { data: 'noPiece' },
                    { data: 'compte' },
                    { data: 'libelle' },
                    { data: 'debit' },
                    { data: 'credit' },
                    { data: 'montant' },
                    { data: 'montantDevise' },
                    { data: 'mon' },
                    { data: 'rang' },
                    { data: 'financementCategorie' },
                    { data: 'commune' },
                    { data: 'plan' },
                    { data: 'journal' },
                    { data: 'marche' },
                ],
               
                createdRow: function (row, data, _) {
                    $(row).attr('compteG-id', data.id);

                    $(row).addClass('select-text');
                    if (data.isLATE) {
                        $(row).attr('style', "background-color: #FF7F7F !important;");
                    }
                },
                columnDefs: [
                    {
                        targets: [-1],
                        className: 'elerfr'
                    }
                ],
                colReorder: {
                    enable: false,
                    fixedColumnsLeft: 1
                },
                deferRender: true,
                dom: 'Bfrtip',
                buttons: ['colvis'],
                initComplete: function () {
                    $(`thead td[data-column-index="${0}"]`).removeClass('sorting_asc').removeClass('sorting_desc');
                    count = 0;
                    this.api().columns().every(function () {
                        var title = this.header();
                        //replace spaces with dashes
                        title = $(title).html().replace(/[\W]/g, '-');
                        var column = this;
                        var select = $('<select id="' + title + '" class="select2" ></select>')
                            .appendTo($(column.footer()).empty())
                            .on('change', function () {
                                //Get the "text" property from each selected data 
                                //regex escape the value and store in array
                                var data = $.map($(this).select2('data'), function (value, key) {
                                    return value.text ? '^' + $.fn.dataTable.util.escapeRegex(value.text) + '$' : null;
                                });

                                //if no data selected use ""
                                if (data.length === 0) {
                                    data = [""];
                                }

                                //join array into string with regex or (|)
                                var val = data.join('|');

                                //search for the option(s) selected
                                column
                                    .search(val ? val : '', true, false)
                                    .draw();
                            });

                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>');
                        });

                        //use column title as selector and placeholder
                        $('#' + title).select2({
                            multiple: true,
                            closeOnSelect: false

                        });

                        //initially clear select otherwise first option is selected
                        $('.select2').val(null).trigger('change');
                    });
                }

            });
            $('#TDB tfoot th').each(function (i) {
                if (i == 0 || i == 19) {
                    $(this).addClass("NOTVISIBLE");
                }
            });
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
//==============================================================================================Export EXCEL===================================================================================

function exportTableToExcel(filename = 'RAS') {
    let downloadLink;

    const dataType = 'application/vnd.ms-excel';

    const tableID = 'TDB';

    const tableSelect = document.getElementById(tableID);

    const tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

    alert("OK");

    // Specify file name
    filename = filename ? filename + '.xls' : 'excel_data.xls';

    // Create download link element
    downloadLink = document.createElement("a");

    document.body.appendChild(downloadLink);
    if (confirm("Voulez-vous le télécharger ?")) {
        if (navigator.msSaveOrOpenBlob) {
            const blob = new Blob(['\ufeff', tableHTML], {
                type: dataType
            });

            navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // Create a link to the file
            downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

            // Setting the file name
            downloadLink.download = filename;

            //triggering the function
            downloadLink.click();
        }
    }
}

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    
    GetAllProjectUser();
});

$(document).on("change", "[compG-list]", () => {
    FillAUXI();
    FillCompteName();
});

$(document).on("change", "[code-project]", () => {
    $('#commercial').html('');
    GetTypeP();
    GetListCodeJournal();
    LoadValidate();
});

$(document).on("change", "[auxi-list]", () => {
    FillCompteName();
});

$(document).on("change", "[codej-list]", () => {
    var code = ListCodeJournal.filter(function (e) { return e.CODE == $(`[codej-list]`).val(); })[0];
    //$(`[codej-libelle]`).val(code.LIBELLE);
});

$(document).on("click", "[data-target]", function () {
    let me = $(this).closest("[data-target]");
    if ($(me).attr("data-type") == "switch_tab") {
        let target = $(`#${$(me).attr("data-target")}`);


        $(`[data-type="switch_tab"]`).each(function (i) {
            if ($(this).hasClass('active')) {

                $(this).removeClass('active');
                $(`#${$(this).attr("data-target")}`).hide();
            }
        });
        $(me).addClass("active");
        $(target).show();


    }
});

$(`[data-action="CreateTxt"]`).click(function () {
    getelementTXT(0);
});

$(`[data-action="CreateTxtCrypter"]`).click(function () {
    getelementTXT(1);
});

$(`[data-action="CreateTxtSend"]`).click(function () {
    getelementTXT(2);
});

$(`[data-action="CreateTxtFTPCrypter"]`).click(function () {
    getelementTXT(3);
});

$('.Checkall').change(function () {

    if ($('.Checkall').prop("checked") == true) {

        $('[compteg-ischecked]').prop("checked", true);
    } else {
        $('[compteg-ischecked]').prop("checked", false);
    }
});
//==============================================================================================ChargeJs===================================================================================

$('[data-action="ChargerJs"]').click(function () {

    let dateDeb = $('#Pdu').val();
    let dateFin = $('#Pau').val();
    let datePaie = $('#Pay').val();

    if (!dateDeb || !dateFin || !datePaie) {
        alert("Veuillez renseigner les dates afin de générer les payements.")
        return;
    }
    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("ChoixBase", baseName);

    formData.append("codeproject", codeproject);
    if (baseName == 2) {
        //compta
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("journal", $('#commercial').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());
        formData.append("devise", false);
        formData.append("etat", $('#etat').val());

        $.ajax({
            type: "POST",
            url: Origin + '/Home/EnvoyeValidatioF',
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
                if (Datas.type == "success") {
                    ListResult = ``;
                    ListResult = Datas.data;

                    const data = [];

                    $.each(ListResult, function (_, v) {
                        data.push({
                            id: v.IDREGLEMENT,
                            dateOrdre: v.dateOrdre === null ? '' : v.dateOrdre,
                            noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                            compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                            libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                            debit: isNullOrUndefined(v.Debit) ? '' : v.Debit,
                            credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                            montant: isNullOrUndefined(v.Montant) ? '' : formatCurrency(String(v.Montant).replace(",", ".")),
                            montantDevise: v.MontantDevise === 0 ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                            mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                            rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                            financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                            commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                            plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                            journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                            marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                            isLATE: v.isLATE
                        });
                    });
                    if (table !== undefined) {
                        table.destroy();
                    }
                    table = $('#TDB').DataTable({
                        data,
                        columns: [
                            { data: 'id' },
                            { data: 'dateOrdre' },
                            { data: 'noPiece' },
                            { data: 'compte' },
                            { data: 'libelle' },
                            { data: 'debit' },
                            { data: 'credit' },
                            { data: 'montantDevise' },
                            { data: 'mon' },
                            { data: 'rang' },
                            { data: 'commune' },
                            { data: 'financementCategorie' },
                            { data: 'plan' },
                            { data: 'journal' },
                            { data: 'marche' },
                        ],
                        createdRow: function (row, data, _) {
                            $(row).attr('compteG-id', data.id);

                            $(row).addClass('select-text');
                            if (data.isLATE) {
                                $(row).attr('style', "background-color: #FF7F7F !important;");
                            }
                        },
                        columnDefs: [
                            {
                                targets: [-1],
                                className: 'elerfr'
                            }
                        ],
                        colReorder: {
                            enable: true,
                            fixedColumnsLeft: 1
                        },
                        deferRender: true,
                        dom: 'Bfrtip',
                        buttons: ['colvis'],
                        initComplete: function () {
                            $(`thead td[data-column-index="${0}"]`).removeClass('sorting_asc').removeClass('sorting_desc');
                            count = 0;
                            this.api().columns().every(function () {
                                var title = this.header();
                                //replace spaces with dashes
                                title = $(title).html().replace(/[\W]/g, '-');
                                var column = this;
                                var select = $('<select id="' + title + '" class="select2" ></select>')
                                    .appendTo($(column.footer()).empty())
                                    .on('change', function () {
                                        //Get the "text" property from each selected data 
                                        //regex escape the value and store in array
                                        var data = $.map($(this).select2('data'), function (value, key) {
                                            return value.text ? '^' + $.fn.dataTable.util.escapeRegex(value.text) + '$' : null;
                                        });

                                        //if no data selected use ""
                                        if (data.length === 0) {
                                            data = [""];
                                        }

                                        //join array into string with regex or (|)
                                        var val = data.join('|');

                                        //search for the option(s) selected
                                        column
                                            .search(val ? val : '', true, false)
                                            .draw();
                                    });

                                column.data().unique().sort().each(function (d, j) {
                                    select.append('<option value="' + d + '">' + d + '</option>');
                                });

                                //use column title as selector and placeholder
                                $('#' + title).select2({
                                    multiple: true,
                                    closeOnSelect: false

                                });

                                //initially clear select otherwise first option is selected
                                $('.select2').val(null).trigger('change');
                            });
                        }
                      
                    });
                    $('#TDB tfoot th').each(function (i) {
                        if (i == 0 || i == 19) {
                            $(this).addClass("NOTVISIBLE");
                        }
                    });
                }
            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });

    } else {
        //BR
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("journal", $('#commercial').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());
        formData.append("etat", $('#etat').val());
        formData.append("devise", false);


        $.ajax({
            type: "POST",
            url: Origin + '/Home/EnvoyeValidatioF',
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
                if (Datas.type == "success") {
                    ListResult = Datas.data
                    content = ``;

                    const data = [];

                    $.each(ListResult, function (_, v) {
                        data.push({
                            id: v.IDREGLEMENT,
                            dateOrdre: v.dateOrdre === null ? '' : v.dateOrdre,
                            noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                            compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                            libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                            debit: isNullOrUndefined(v.Debit) ? '' : v.Debit,
                            credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                            montant: isNullOrUndefined(v.Montant) ? '' : formatCurrency(String(v.Montant).replace(",", ".")),
                            montantDevise: v.MontantDevise === 0 ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                            mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                            rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                            financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                            commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                            plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                            journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                            marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                            isLATE: v.isLATE
                        });
                    });

                    if (table !== undefined) {
                        table.destroy();
                    }
                    table = $('#TDB').DataTable({
                        data,
                        columns: [
                            { data: 'id' },
                            { data: 'dateOrdre' },
                            { data: 'noPiece' },
                            { data: 'compte' },
                            { data: 'libelle' },
                            { data: 'debit' },
                            { data: 'credit' },
                            { data: 'montant' },
                            { data: 'montantDevise' },
                            { data: 'mon' },
                            { data: 'rang' },
                            { data: 'financementCategorie' },
                            { data: 'commune' },
                            { data: 'plan' },
                            { data: 'journal' },
                            { data: 'marche' },
                        ],
                        
                        createdRow: function (row, data, _) {
                            $(row).attr('compteG-id', data.id);

                            $(row).addClass('select-text');
                            if (data.isLATE) {
                                $(row).attr('style', "background-color: #FF7F7F !important;");
                            }
                        },
                        columnDefs: [
                            {
                                targets: [-1],
                                className: 'elerfr'
                            }
                        ],
                        colReorder: {
                            enable: true,
                            fixedColumnsLeft: 1
                        },
                        deferRender: true,
                        dom: 'Bfrtip',
                        buttons: ['colvis'],
                        initComplete: function () {
                            $(`thead td[data-column-index="${0}"]`).removeClass('sorting_asc').removeClass('sorting_desc');
                            count = 0;
                            this.api().columns().every(function () {
                                var title = this.header();
                                //replace spaces with dashes
                                title = $(title).html().replace(/[\W]/g, '-');
                                var column = this;
                                var select = $('<select id="' + title + '" class="select2" ></select>')
                                    .appendTo($(column.footer()).empty())
                                    .on('change', function () {
                                        var data = $.map($(this).select2('data'), function (value, key) {
                                            return value.text ? '^' + $.fn.dataTable.util.escapeRegex(value.text) + '$' : null;
                                        });

                                        //if no data selected use ""
                                        if (data.length === 0) {
                                            data = [""];
                                        }

                                        //join array into string with regex or (|)
                                        var val = data.join('|');

                                        //search for the option(s) selected
                                        column
                                            .search(val ? val : '', true, false)
                                            .draw();
                                    });

                                column.data().unique().sort().each(function (d, j) {
                                    select.append('<option value="' + d + '">' + d + '</option>');
                                });

                                //use column title as selector and placeholder
                                $('#' + title).select2({
                                    multiple: true,
                                    closeOnSelect: false

                                });

                                //initially clear select otherwise first option is selected
                                $('.select2').val(null).trigger('change');
                            });
                        }
                    });
                    $('#TDB tfoot th').each(function (i) {
                        if (i == 0 || i == 19) {
                            $(this).addClass("NOTVISIBLE");
                        }
                    });
                }
            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });
    }
});
//==============================================================================================Checked===================================================================================

$('[data-action="GetElementChecked"]').click(function () {
    let checkList = $(`[compteg-ischecked]:checked`).closest("tr");
    let list = [];

    $.each(checkList, (_, v) => {
        list.push($(v).attr("compteG-id"));
    });

    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("listCompte", list);
    formData.append("baseName", baseName);
    formData.append("journal", $('#commercial').val());
    formData.append("devise", false);
    let listid = list.splice(',');
    formData.append("datein", $('#Pdu').val());
    formData.append("dateout", $('#Pau').val());
    formData.append("comptaG", $('#comptaG').val());
    formData.append("auxi", $('#auxi').val());
    formData.append("auxi1", $('#auxi').val());
    formData.append("dateP", $('#Pay').val());
    formData.append("etat", $('#etat').val());

    $.ajax({
        type: "POST",
        url: Origin + '/Home/ValidationsEcrituresF',
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

            reglementresult = ``;
            reglementresult = Datas.data;

            for (let i = 0; i < checkList.length; i += 1) {
                table.row($(checkList[i])).remove().draw();
            }

            $.each(listid, function (_, x) {
                $.each(reglementresult, function (_, v) {
                    if (v != null) {
                        if (v.No == x) {
                            data.push({
                                id: v.IDREGLEMENT,
                                dateOrdre: v.dateOrdre ? '' : v.dateOrdre,
                                noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                                compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                                libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                                debit: isNullOrUndefined(v.Debit) ? '' : v.Debit,
                                credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                                montant: isNullOrUndefined(v.Montant) ? '' : formatCurrency(String(v.Montant).replace(",", ".")),
                                montantDevise: v.MontantDevise === 0 ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                                mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                                rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                                financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                                commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                                plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                                journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                                marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                                isLATE: v.isLATE
                            });
                        }
                    }
                });

                table = $('#TDB').DataTable({
                    data,
                    columns: [
                        { data: 'id' },
                        { data: 'date' },
                        { data: 'noPiece' },
                        { data: 'compte' },
                        { data: 'libelle' },
                        { data: 'montantDevise' },
                        { data: 'mon' },
                        { data: 'rang' },
                        { data: 'financementCategorie' },
                        { data: 'commune' },
                        { data: 'plan' },
                        { data: 'journal' },
                        { data: 'marche' },
                        { data: 'status' }
                    ],
                    colReorder: {
                        enable: true,
                        // fixedColumnsLeft: 1
                    },
                    deferRender: true,
                    createdRow: function (row, data, _) {
                        $(row).attr('compteG-id', data.id);
                    },
                    // initComplete: function () {
                    //     $(`thead td[data-column-index="${0}"]`).removeClass('sorting_asc').removeClass('sorting_desc');
                    // }
                });
            });
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });

});
//==============================================================================================Anomalie===================================================================================

$('[data-action="GetAnomalieListes"]').click(function () {

    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", baseName);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetAnomalieBack',
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

            ListResultAnomalie = "";
            contentAnomalies = ``;
            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);

                return;
            }
            if (Datas.type == "success") {
                ListResultAnomalie = Datas.data;
                $.each(ListResultAnomalie, function (k, v) {
                    contentAnomalies += `<tr compteG-id="${v.IDREGLEMENT}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td><td>${v.IDREGLEMENT}</td>
                        <td>${v.dateOrdre}</td>
                        <td>${v.NoPiece}</td>
                        <td>${v.Compte}</td>
                        <td>${v.Libelle}</td>
                        <td>${v.Debit}</td>
                        <td>${v.Credit}</td>
                        <td>${v.MontantDevise}</td>
                        <td>${v.Mon}</td>
                        <td>${v.Rang}</td>
                        <td>${v.FinancementCategorie}</td>
                        <td>${v.Commune}</td>
                        <td>${v.Plan6}</td>
                        <td>${v.Journal}</td>
                        <td>${v.Marche}</td>
                    </tr>`

                });
                $('.anomalieslist').html(contentAnomalies);
            }

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
});

var baseName = "2";

