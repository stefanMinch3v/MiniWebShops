// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

(function () {
    function disableF5(e) {
        if ((e.which || e.keyCode) == 116) {
            e.preventDefault();
        }
    }

    $(document).on("keydown", disableF5);
})();