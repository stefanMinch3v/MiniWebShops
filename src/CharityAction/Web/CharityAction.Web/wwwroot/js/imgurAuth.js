(function () {
    const imgurCookie = Cookies.get('MySecurityToken');

    if (!imgurCookie) {
        const urlPath = window.location.hash;
        const regex = /^(#access_token=)([a-z0-9]+)/gm;
        const arrayToken = regex.exec(urlPath);

        if (!arrayToken) {
            alert("Please login to imgur!/Моля логнете се в imgur през бутона!");

            const createBtn = document.getElementsByClassName('create-event')[0];
            createBtn.style.display = 'none';

            const myForm = Array.from(document.getElementsByClassName('js-block'));
            myForm.map(e => {
                e.disabled = true;
                e.style.cursor = 'no-drop';
            });

            return;
        } 

        const imgurBtn = document.getElementsByClassName('imgurBtn')[0];
        imgurBtn.style.display = 'none';

        const finalToken = arrayToken[2];
        const encodedToken = btoa(finalToken);

        Cookies.set('MySecurityToken', `${encodedToken}`, { expires: 7, path: '' });
        alert("Successfully connected !/Свързването с профила успешно!");
    }

    $('.create-event').on('click', function () {

        let isValidForm = true;
        $('.form-field').each(function () {
            if ($(this).val() === '') {
                isValidForm = false;
            }
        });

        if (isValidForm) {
            $('.container-fluid')
                .fadeIn(1000)
                .css({ 'opacity': '0.3' });

            $('.spinner')
                .fadeIn(1000)
                .css({ 'visibility': 'visible', 'display': 'block' });
        }
    });
})();