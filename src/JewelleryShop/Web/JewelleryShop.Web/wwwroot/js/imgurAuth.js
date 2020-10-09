$(function () {
    const imgurCookie = Cookies.get('MySecurityToken');
    if (!imgurCookie) {
        const urlPath = window.location.hash;
        const regex = /^(#access_token=)([a-z0-9]+)/gm;
        const arrayToken = regex.exec(urlPath);

        if (!arrayToken) {
            alert("Please login to imgur!/Моля логнете се в imgur през бутона!");
        } else {
            const finalToken = arrayToken[2];
            const encodedToken = btoa(finalToken);
            Cookies.set('MySecurityToken', `${encodedToken}`, { expires: 7, path: '' });
            alert("Successfully connected !/Свързването с профила успешно!");
        }
    }

    $('.create-product').click(function () {
        const spinner = $('.spinner');
        const image = $('#image');
        const title = $('#title');
        const description = $('#description');
        const price = $('#price');
        const purchNumber = $('#purchase-number');
        const createBtn = $('#btn-create');

        if (title.val()
            && description.val()
            && price.val()
            && purchNumber.val()) {
            image
                .fadeIn(2000)
                .css({ 'background-color': '#3CBC8D', 'color': 'white' });
            title
                .fadeIn(2000)
                .css({ 'background-color': '#3CBC8D', 'color': 'white' });
            description
                .fadeIn(2000)
                .css({ 'background-color': '#3CBC8D', 'color': 'white' });
            price
                .fadeIn(2000)
                .css({ 'background-color': '#3CBC8D', 'color': 'white' });
            purchNumber
                .fadeIn(2000)
                .css({ 'background-color': '#3CBC8D', 'color': 'white' });
            createBtn
                .fadeIn(2000)
                .css('display', 'none');

            spinner
                .fadeIn(2000)
                .css({ 'visibility': 'visible', 'display': 'block' });
        }
    });
});
//$('#my-form').ajaxSend(function () {
//    spinner.css({ 'visibility': 'hidden', 'display': 'none' });
//});
