$(".game-button").click(function () {
    $(".game-button").css("display", "none");
    $("#loader").css("display", "block");
    $.ajax({
        url: "/queue/QueueMe", success: function (result) {
            var redirectUrl = "http://localhost:49816/Match/Index?id=" + result.Object.MatchId;
            window.location = redirectUrl;
        }
    });
});
