function getDecision(callback) {
    $.get("/decision", function (data) {
        console.log(data);
        callback(data && data.decision === true);
    });
}

$(document).ready(function () {
    init3d();

    $(".button").click(function () {
        var loader = $.colorbox({
            html: "<div style='text-align:center; padding: 50px;'><h1>Steve is deciding!</h1></div>",
            transition: "none",
            title: "Hmmm..."
        });

        getDecision(function (decision) {
            var answer = decision ? "YES!" : "NO!";
            var url = decision ? "/images/yes.png" : "/images/no.png";

            $.colorbox({
                href: url,
                transition: "fade",
                speed: 600,
                title: answer,
                scalePhotos: true,
                width: "500px"
            });
        });
    });
});