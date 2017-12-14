$(document).ready(function () {
    var initState = function () {
        $.ajax({
            url: "/Match/checkMatchStatus?matchId=" + $("#matchId").val(), success: function (result) {
                switch (result.Data.MatchState) {
                    case 0:
                        if ($('#isOpponent1').val() === 'False') {
                            betListener();
                        }
                        break;
                    case 1:
                        $(".proposedBet").val(result.Data.PrizePool);
                        $(".btnHandleBet").css("display", "inline-block");
                        if ($('#isOpponent1').val() === 'True') {
                            responseListener();
                        }
                        break;
                    case 2:
                        break;
                    case 3:
                        $(".proposedBet").val(result.Data.PrizePool);
                        $(".btnProposeBet").css("display", "none");
                        $(".proposedBet").prop('disabled', true);
                        $(".btnRoll").prop('disabled', false).removeClass("inactive-button");
                        break;
                    case 3:
                        $(".proposedBet").val(result.Data.PrizePool);
                        $(".btnProposeBet").css("display", "none");
                        $(".proposedBet").prop('disabled', true);
                        $(".btnRoll").prop('disabled', true).addClass("inactive-button");
                        $(".resetGame").prop('disabled', false);
                        break;
                    case 4:
                        $(".proposedBet").val(result.Data.PrizePool);
                        $(".btnProposeBet").css("display", "none");
                        $(".proposedBet").prop('disabled', true);
                        $(".btnRoll").prop('disabled', true).addClass("inactive-button");
                        $(".resetGame").prop('disabled', true);
                        setInterval(countDown(), 1000);
                        break;
                }
            }
        });
    }
    initState();
});


$(".btnProposeBet").click(function () {
    responseListener();
});

$(".btnAacceptBet").click(function () {
    setTimeout(function () {
        subtractBalance($(".proposedBet").val());
        console.log($(".proposedBet").val())
    }, 300);
    respondToBet(true);
});

$(".btnDeclineBet").click(function () {
    respondToBet(false);
});
    
$(".btnRoll").click(function () {
    $(".btnRoll").prop('disabled', true).addClass("inactive-button");
    $.ajax({
        url: "/Match/Roll?matchId=" + $("#matchId").val(), success: function (result) {
            console.log(result);
            $("#Opponent1Face").text(result.Opponent1Roll);
            $("#Opponent2Face").text(result.Opponent2Roll);
            if (result.Opponent1Roll === result.Opponent2Roll) {
                resetRoll();
                $(".btnRoll").prop('disabled', false).removeClass("inactive-button");
                $("#Winner").text("Draw");

            } else {
                $("#Winner").text(result.Winner);
                setInterval(countDown, 1000);
            }
        }
    });
});

var countDown = function () {
    $(".CountDown").css("display", "block");
    $('.counter').each(function () {
        var count = parseInt($(this).html());
        if (count !== 0) {
            $(this).text(count - 1);
        } else {
            // Schedule the update to happen once every second
            $.ajax({
                url: "/Match/TransferMatch?matchId=" + $("#matchId").val(), success: function () {
                    window.location = "http://localhost:49816/Home/Index";
                }
            });
        }
    });
};
var respondToBet = function (response) {
    $(".btnHandleBet").css("display", "none");
    $.ajax({
        url: "/Match/RespondBet?accepted=" + response + "&matchId=" + $("#matchId").val(), success: function (result) {
            if (!response) {
                betListener();
                $(".proposedBet").val("");
            } else {
                $(".btnRoll").prop('disabled', false);
            }
        }
    });
}

var betListener = function () {
    $.ajax({
        url: "/Match/RecieveBet?matchId=" + $("#matchId").val(), success: function (result) {
            $(".proposedBet").val(result.Amount);
            $(".btnHandleBet").css("display", "inline-block");
        }
    });
}

var responseListener = function () {
    $(".btnProposeBet").css("display", "none");
    $(".proposedBet").prop('disabled', true);

    $.ajax({
        url: "/Match/ProposeBet?matchId=" + $("#matchId").val() + "&amount=" + $(".proposedBet").val(), success: function (result) {
            if (result.Status === "ACC") {
                subtractBalance(result.Amount);
                $(".btnRoll").prop('disabled', false);
            } else if (result.Status === "DEC") {
                $(".btnProposeBet").css("display", "inline-block");
                $(".proposedBet").prop('disabled', false);
            }
        }
    });
}
var resetRoll = function () {
    $.ajax({
        url: "/Match/ResetRoll?matchId=" + $("#matchId").val(), success: function () {
            $(".btnRoll").prop('disabled', false);
        }
    });
}

var subtractBalance = function (amount) {
    var balance = $("#nav--profile-balance").text();
    var num = parseInt(balance)
    var result = num - amount;
    $("#nav--profile-balance").text(result)
}