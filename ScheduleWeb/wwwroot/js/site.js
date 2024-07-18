$(document).ready(function () {
    $(".showButton").each(function () {
        $(this).click(function () {
            var container = $(this).closest('td').find('.show-container');
            container.slideToggle("slow");
            $(".show-container").not(container).slideUp("slow");
        });
    });
});
