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
    $("#result").removeClass("copied error success").text("");
    focus();
}

function pasteNumber() {
    navigator.clipboard.readText().then((text) => {
        porextenso(text);
        $("#number").val(text);
        focusAndSelect();
    });
}

$(function () {
    var fireEmpty = false;
    (function emptyLoop() {
        setTimeout(function () {
            if (document.getElementById("number").value) {
                fireEmpty = true;
            } else if (fireEmpty) {
                $("#number").trigger("empty");
                fireEmpty = false;
            }
            emptyLoop();
        }, 80);
    })();

    $(".showTooltip").tooltip();
    $("#number").on("input", function (event) {
        let sanitized = this.value.replace(/[eE+\-.,]/g, "");
        porextenso(sanitized);
    }).on("keydown", function (event) {
        if ((event.key === 'Backspace' || event.keyCode === 8) && this.value.length === 1) {
            $("#result").removeClass("copied error success");
        }
    }).on("empty", function () {
        $("#result").removeClass("copied error success");
    });
    focus();
});
