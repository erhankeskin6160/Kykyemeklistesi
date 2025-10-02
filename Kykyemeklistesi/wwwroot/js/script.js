$(document).ready(function () {
    console.log("deneme");


    $("#excel").on("click", function () {
        $(this).append('<span class="loading-text ms-2">İndiriliyor...</span>');
        setTimeout(() => {
            $(".loading-text").fadeOut(500, function () { $(this).remove(); });
        }, 3000);
    });

    $("#excel").on("click", function () {
        $(this).addClass("shake");

        setTimeout(() => {
            $(this).removeClass("shake");
        }, 500);
    });



});
