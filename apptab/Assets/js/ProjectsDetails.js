let originalTitle;

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem('user'));

    if (!User) {
        window.location = User.origin;
    }

    Origin = User.origin;

    originalTitle = $('#title').val();
});

function successfulRedirect() {
    alert("Modification avec succès!");

    window.location = Origin + '/Projects/AllProjects';
}

$('[data-action="update-project"]').on('click', () => {
    const title = $('#title').val();

    if (title === originalTitle) {
        successfulRedirect();

        return;
    }

    const payload = {
        login: User.LOGIN,
        password: User.PWD,
        id: $('#id_').val(),
        title
    }

    $.ajax({
        type: 'POST',
        async: true,
        url: Origin + '/Projects/UpdateProject',
        contentType: 'application/json',
        datatype: 'json',
        data: JSON.stringify({ ...payload }),
        beforeSend: function () {
            loader.removeClass('display-none');
        },
        complete: function () {
            loader.addClass('display-none');
        },
        success: function () {
            successfulRedirect();
        },
        Error: function (_, e) {
            alert(e);
        }
    });
});
