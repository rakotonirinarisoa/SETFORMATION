function formatCurrency(amount) {
    return new Intl.NumberFormat("fr-FR").format(amount);
}

function formatDate(date) {
    if (!dayjs(date).isValid()) {
        return '';
    }

    return dayjs(date).format('DD/MM/YYYY');
}

function formatDateRFR(date) {
    if (!dayjs(date).isValid()) {
        return '';
    }

    return dayjs(date).format('YYYY-MM-DD');
}

function tableToExcel(tableId, name, callback = undefined) {
    let table;
    const uri = 'data:application/vnd.ms-excel;base64,'
        ,
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta http-equiv="content-type" content="application/vnd.ms-excel; charset=UTF-8"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        , base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)))
        }
        , format = function (s, c) {
            return s.replace(/{(\w+)}/g, function (m, p) {
                return c[p];
            })
        }

    if (!tableId.nodeType) {
        table = document.getElementById(tableId);
    }
    //const er = table.innerHTML.replace(/^0/g, "'0")
    const ctx = { worksheet: name || 'Worksheet', table: table.innerHTML };

    if (callback) {
        callback();
    }

    const a = document.createElement('a');

    a.href = uri + base64(format(template, ctx));

    a.download = name + '.xls';

    a.click();
}

function isNullOrUndefined(input) {
    return input === null || input === undefined;
} 