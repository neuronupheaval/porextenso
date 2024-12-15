function focus() {
    $("#number").trigger("focus");
}

function select() {
    document.getElementById("number").select();
}

function focusAndSelect() {
    focus();
    select();
}

function sanitize(input) {
    return input.replace(/\D/g, "");
}

function porextenso(number) {
    $.ajax({
        "data": { "number": number },
        "dataType": "json",
        "error": function () {
            $("#result").removeClass("success").addClass("error").text("---> erro <---");
            focusAndSelect();
        },
        "method": "post",
        "success": function (result) {
            $("#result").text(result);
            result && $("#result").removeClass("error").addClass("success");
            !result && $("#result").removeClass("error copied");
        }
    });
}

function copy() {
    if ($("#result").text()) {
        navigator.clipboard.writeText($("#result").text());

        $("#result").removeClass("success").addClass("copied");
        setTimeout(function () {
            $("#result").removeClass("copied").addClass("success");
            focusAndSelect();
        }, 300);
    }
}

function clearNumber() {
    $("#number").val("");
}

function pasteNumber() {
    navigator.clipboard.readText().then((text) => {
        let sanitized = sanitize(text);
        porextenso(sanitized);
        $("#number").val(sanitized);
        focusAndSelect();
    });
}

$(function () {
    var fireEmpty = true;
    var $number = document.getElementById("number");
    var $jNumber = $("#number");

    (function emptyLoop() {
        setTimeout(function () {
            if ($number.value) {
                fireEmpty = true;
            } else if (fireEmpty) {
                $jNumber.trigger("empty");
                fireEmpty = false;
            }
            emptyLoop();
        }, 80);
    })();

    $(".showTooltip").tooltip();

    $jNumber.on("input", function (event) {
        if (this.value) {
            let sanitized = sanitize(this.value);
            porextenso(sanitized);
            if (sanitized !== this.value) {
                this.value = sanitized;
                // begin hack
                this.type = "text";
                this.setSelectionRange(sanitized.length, sanitized.length);
                this.type = "number";
                // end hack
            }
        }
    }).on("empty", function (event) {
        focus();
        $("#result").removeClass("copied error success").text("");
    });
});
