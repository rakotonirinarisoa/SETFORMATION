function setCurrentRoute(link) {
    sessionStorage.setItem('j3rGjf', link);
    $('nav').find(`a[href="${link}"].nav-link`).find('p').css({ 'color': 'red' });
}

function getCurrentLink() {
    const currentRoute = sessionStorage.getItem('j3rGjf');

    if (currentRoute) {
        const elmt = $('nav').find(`a[href="${currentRoute}"].nav-link`);

        //elmt.find('p').css({ 'color': 'red' });
        elmt.find('p').addClass('image-clignote');

        return currentRoute;
    }

    return '';
}

$(document).ready(() => {
    getCurrentLink();
});

$('nav').find(`a.nav-link`).on('click', (e) => {
    $('nav').find(`a.nav-link`).find('p').css({ 'color': 'title' });

    const id = $(e.currentTarget).prop('href');

    const str = id.split(User.origin);

    setCurrentRoute(`..${str[1]}`);
});