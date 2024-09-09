$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = User.origin;
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    GetListEvenements();
});

//function get_calendar_height() {
//    return $(window).height() - 250;
//}

function GetListEvenements() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDPROJET", User.IDPROJET);
    //$(window).resize(function () {
    //    $('#calendar').fullCalendar('option', 'height', get_calendar_height());
    //});


    $.ajax({
        type: "POST",
        url: Origin + '/Calendar/GetAllEvents',
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
            var code = [];
            $.each(Datas.data, function (k, v) {
                let type = 'engagements';
                let typeT = 'ENGAGEMENT';
                if (v.TYPE == 1) { type = 'paiements'; typeT = 'PAIEMENT'; }
                if (v.TYPE == 3) { type = 'avances'; typeT = 'AVANCE'; }
                let etat = 'Tris';
                let color = '#1E8BFF'
                if (v.ETAT == 1) { etat = 'Validation'; color = '#3EB059' }
                if (v.ETAT == 2) { etat = 'Annulation'; color = '#DF4857' }
                if (v.ETAT == 3) { etat = 'Transfert vers SIIGFP'; color = '#FFC107' }
                if (v.ETAT == 4) { etat = 'Intégration dans SIIGFP'; color = '#17A2B8' }

                code.push(
                {
                    start: (formatDateRFR(v.DATE)),
                    //constraint: 'businessHours',
                    backgroundColor: `${color}`,
                    borderColor: `${color}`,
                    extendedProps: {
                        title: `${v.SOA} : ${v.PROJET} : ${typeT} : ${v.USER}`,
                        description: `${etat} de ${v.COUNT} ${type} par ${v.USER}`,
                    },
                });
            });

            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl, {
                editable: false,
                selectable: true,

                //businessHours: true,

                dayMaxEvents: true, // allow "more" link when too many events
                headerToolbar: {
                    left: 'title prev,next today',
                    center: '',
                    right: 'listWeek,listMonth,listYear,dayGridMonth,timeGridWeek,timeGridDay'
                },
                views: {
                    listWeek: { buttonText: 'Tache par semaine' },
                    listMonth: { buttonText: 'Tache par mois' },
                    listYear: { buttonText: 'Tache par année' },

                    dayGridMonth: { buttonText: 'Mois' },
                    timeGridWeek: { buttonText: 'Semaine' },
                    timeGridDay: { buttonText: 'Jour' },
                },
                initialView: 'dayGridMonth',

                //contentHeight: 700,
                /*height: get_calendar_height(),*/

                events: code,
                eventDidMount: function (info) {

                    //var tooltip = new Tooltip(info.el, {
                    //    title: info.event.extendedProps.description,
                    //    placement: 'top',
                    //    trigger: 'hover',
                    //    container: 'body'
                    //});
                    /*info.event.extendedProps*/
                    //console.log(info.event.extendedProps);

                    let enventTitle = info.el.querySelector('.fc-event-title');
                    let eventTitleList = info.el.querySelector('.fc-list-event-title');

                    //console.log(info.event);
                    //console.log(info);

                    if (enventTitle === null) {
                        eventTitleList.innerHTML += ('<a style="font-size: 10px">' + info.event.extendedProps.title + '<div class="hr-line-solid-no-margin"></div ><span style="font-size: 7px">' + info.event.extendedProps.description + '</span></a>');
                    }
                    else {
                        enventTitle.innerHTML += ('<a style="font-size: 10px">' + info.event.extendedProps.title + '<div class="hr-line-solid-no-margin"></div ><span style="font-size: 7px">' + info.event.extendedProps.description + '</span></a>');
                    }
                }
            });

            calendar.setOption('locale', 'FR');
            calendar.updateSize();
            
            calendar.render();
            /*console.log(calendar);*/
        },
        error: function (e) {
            alert("Problème de connexion. ");
        }
    });
}
