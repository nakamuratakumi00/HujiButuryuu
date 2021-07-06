function watchScroll() {
    var animSpeed = "fast";
    var now = $(window).scrollTop();

    if (now > 150) {
        $("#TopLinkBox").fadeIn(animSpeed);
    } else {
        $("#TopLinkBox").fadeOut(animSpeed);
    }
}

$(function () {

    $(window).scroll(function () {
        watchScroll();
    });

    $("#TopLink").click(function () {
        $("html,body").animate({
            scrollTop: 0
        }, "slow"
        );
    });

    watchScroll();
});