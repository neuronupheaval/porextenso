function focus() {
    $("#number").trigger("focus").trigger("click");
}

function select() {
    $("#number").val() && $("#number")[0].select();
}

function focusAndSelect() {
    focus();
    select();
}

function sanitize(input) {
    return input.replace(/\D/g, "");
}

function startButtonClicked() {
    $("#start").remove();
    $("#app").removeClass("off").addClass("on");
    focus();
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
            $("#result").text(result).removeClass("error copied");
            result && $("#result").addClass("success");
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
    focus();
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

    $.widget("ui.tooltip", $.ui.tooltip, {
        "options": {
            "show": {
                "effect": "fade",
                "delay": 400
            },
            "open": function (event, ui) {
                var self = this;
                setTimeout(function () {
                    $(self).tooltip("close");
                }, 8500);
            }
        }
    });
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
        $("#result").removeClass("copied error success").text("");
    });
});
