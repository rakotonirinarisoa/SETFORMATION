var table = undefined;
var baseName = "2";

let arr = [];

function checkdel(id) {
    $('.Checkall').prop("checked", false);
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
                return;
            }
            etaCode = `<option value = "Tous"> Tous</option> `;
            console.log(listEtat);
            $.each(listEtat, function (k, v) {
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

function GetListCompG() {
    let formData = new FormData();

    let codeproject = $("#Fproject").val();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("codeproject", codeproject);
    //formData.append("baseName", id);

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
                window.location = window.location.origin;
                return;
            }

            let code = ``;

            ListCompteG = Datas.data;

            $.each(ListCompteG, function (k, v) {
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

function GetListCodeJournal() {
    let formData = new FormData();

    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDSOCIETE);
    //formData.append("baseName", id);

    console.log(codeproject)

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

            const Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                //window.location = window.location.origin;
                return;
            }

            let code = ``;
            ListCodeJournal = Datas.data;
            console.log(ListCodeJournal);
            $.each(ListCodeJournal, function (k, v) {
                code += `
                    <option value="${v.CODE}">${v.CODE}</option>
                `;
            });
            $(`[codej-list]`).html('');
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

function GetAllProjectUser() {
    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);

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
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function FillAUXI() {
    var list = ListCompteG.filter(x => x.COGE == $(`[compG-list]`).val()).pop();

    let code = `<option value="Tous"> Tous</option> `;
    $.each(list.AUXI, function (k, v) {
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

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    emptyTable();
    GetAllProjectUser();
});

$(document).on("change", "[code-project]", () => {
    $('#commercial').html('');
    GetTypeP();
    GetListCodeJournal();
    emptyTable();
});
function emptyTable() {
    const data = [];

    if (table !== undefined) {
        table.destroy();
    }

    table = $('#TDB_OPA').DataTable({
        data,
        colReorder: {
            enable: true,
            fixedColumnsLeft: 1
        },
        deferRender: true,
        dom: 'Bfrtip',
        buttons: ['colvis'],
    });
}

$(document).on("change", "[compG-list]", () => {
    FillAUXI();
    FillCompteName();
});

$(document).on("change", "[auxi-list]", () => {
    FillCompteName();
});

$(document).on("change", "[codej-list]", () => {
    var code = ListCodeJournal.filter(function (e) { return e.CODE == $(`[codej-list]`).val(); })[0];
    $(`[codej-libelle]`).val(code.LIBELLE);
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

    if ( !dateDeb || !dateFin || !datePaie)  {
        alert("Veuillez renseigner les dates afin de générer les payements.")
        return;
    }
    let formData = new FormData();
    let codeproject = $("#Fproject").val();
    formData.append("codeproject", codeproject);

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("ChoixBase", baseName);
    
    if (baseName == "2") {
       
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("journal", $('#commercial').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());

      

        if ($('#ChkDevise').prop("checked") == true) {
            formData.append("devise", true);
        } else {
            formData.append("devise", false);
        }

        $.ajax({
            type: "POST",
            url: Origin + '/Home/Getelementjs',
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
                    listResult = Datas.data;

                    const data = [];

                    arr = data;

                    $.each(listResult, function (_, v) {
                        data.push({
                            checkbox: '',
                            id: isNullOrUndefined(v.No) ? '' : v.No,
                            dateOrdre: isNullOrUndefined(v.dateOrdre) ? '' : v.dateOrdre,
                            noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                            compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                            libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                            debit: isNullOrUndefined(v.Debit) ? '' : formatCurrency(String(v.Debit).replace(",", ".")),
                            credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                            montant: isNullOrUndefined(v.Montant) ? '' : formatCurrency(String(v.Montant).replace(",", ".")),
                            montantDevise: isNullOrUndefined(v.MontantDevise) ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                            mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                            rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                            financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                            commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                            plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                            journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                            marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                            estAvance: v.Avance,
                            numeroliquidations: isNullOrUndefined(v.NUMEROLIQUIDATION) ? '' : v.NUMEROLIQUIDATION 
                        });
                    });

                    if (table !== undefined) {
                        table.destroy();
                    }

                    table = $('#TDB_OPA').DataTable({
                        data,
                        columns: [
                            {
                                data: 'checkbox',
                                render: function () {
                                    return `
                                        <input type="checkbox" name="checkprod" compteg-ischecked onchange="checkdel()" />
                                    `;
                                },
                                orderable: false
                            },
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
                            { data: 'numeroliquidations' }
                        ],
                        createdRow: function (row, data, _) {
                            $(row).attr('compteG-id', data.id);
                            $(row).addClass('select-text');
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
                          $('#TDB_OPA tfoot th').each(function (i) {
                              if (i == 0 ) {
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

        let codeproject = $("#Fproject").val();
        formData.append("codeproject", codeproject);

        if ($('#ChkDevise').prop("checked") == true) {
            formData.append("devise", true);
        } else {
            formData.append("devise", false);
        }

        $.ajax({
            type: "POST",
            url: Origin + '/Home/getelementjsBR',
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
                    listResult = Datas.data
                    content = ``;

                    const data = [];

                    arr = data;

                    $.each(listResult, function (_, v) {
                        console.log(v);

                        data.push({
                            checkbox: '',
                            id: v.No,
                            date: isNullOrUndefined(v.Date) ? '' : v.Date,
                            noPiece: isNullOrUndefined(v.NoPiece) ? '' : v.NoPiece,
                            compte: isNullOrUndefined(v.Compte) ? '' : v.Compte,
                            libelle: isNullOrUndefined(v.Libelle) ? '' : v.Libelle,
                            debit: isNullOrUndefined(v.Debit) ? '' : formatCurrency(String(v.Debit).replace(",", ".")),
                            credit: isNullOrUndefined(v.Credit) ? '' : formatCurrency(String(v.Credit).replace(",", ".")),
                            montant: isNullOrUndefined(v.Montant) ? '' : formatCurrency(String(v.Montant).replace(",", ".")),
                            montantDevise: isNullOrUndefined(v.MontantDevise) ? '' : formatCurrency(String(v.MontantDevise).replace(",", ".")),
                            mon: isNullOrUndefined(v.Mon) ? '' : v.Mon,
                            rang: isNullOrUndefined(v.Rang) ? '' : v.Rang,
                            financementCategorie: isNullOrUndefined(v.FinancementCategorie) ? '' : v.FinancementCategorie,
                            commune: isNullOrUndefined(v.Commune) ? '' : v.Commune,
                            plan: isNullOrUndefined(v.Plan6) ? '' : v.Plan6,
                            journal: isNullOrUndefined(v.Journal) ? '' : v.Journal,
                            marche: isNullOrUndefined(v.Marche) ? '' : v.Marche,
                            estAvance: v.Avance,
                            numeroliquidations: v.Mandat 
                        });
                    });

                    if (table !== undefined) {
                        table.destroy();
                    }

                    table = $('#TDB_OPA').DataTable({
                        data,
                        columns: [
                            {
                                data: 'checkbox',
                                render: function () {
                                    return `
                                        <input type="checkbox" name="checkprod" compteg-ischecked onchange="checkdel()" />
                                    `;
                                },
                                orderable: false
                            },
                            { data: 'id' },
                            { data: 'date' },
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
                            { data: 'numeroliquidations' }
                        ],
                        createdRow: function (row, data, _) {
                            $(row).attr('compteG-id', data.id);
                            $(row).addClass('select-text');
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
                    $('#TDB_OPA tfoot th').each(function (i) {
                        if (i == 0) {
                            $(this).addClass("NOTVISIBLE");
                        }
                    });
                }
            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });

        $('.afb160').empty()
    }
});
//==============================================================================================CHECK===================================================================================

$('[data-action="GetElementChecked"]').on('click', () => {
    let checkList = $(`[compteg-ischecked]:checked`).closest("tr");

    let list = [];
    if (baseName == "2") {
        for (let i = 0; i < checkList.length; i += 1) {
            const id = $(checkList[i]).attr("compteG-id");

            const item = arr.find(item => item.id === Number(id));
            list.push({
                id,
                estAvance: item.estAvance
            });
        }
    } else {
        for (let i = 0; i < checkList.length; i += 1) {
            const id = $(checkList[i]).attr("compteG-id");

            const item = arr.find(item => item.id === id);
            list.push({
                id,
                estAvance: item.estAvance
            });
        }
    }
   

    let formData = new FormData();

    let codeproject = $("#Fproject").val();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    formData.append("listCompte", JSON.stringify(list));
    formData.append("codeproject", codeproject);
    //formData.append("baseName", baseName);
    formData.append("journal", $('#commercial').val());
    formData.append("devise", false);
    formData.append("datein", $('#Pdu').val());
    formData.append("dateout", $('#Pau').val());
    formData.append("comptaG", $('#comptaG').val());
    formData.append("auxi", $('#auxi').val());
    formData.append("auxi1", $('#auxi').val());
    formData.append("dateP", $('#Pay').val());
    formData.append("etat", $('#etat').val());

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCheckedCompte',
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
        success: function () {
            for (let i = 0; i < checkList.length; i += 1) {
                table.row($(checkList[i])).remove().draw();
            }
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
});
