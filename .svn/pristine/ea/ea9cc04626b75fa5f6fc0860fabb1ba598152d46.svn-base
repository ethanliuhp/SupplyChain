
function loadProgress() {
    var mainFramesetChildCon = parent.document.getElementById("mainFramesetChildCon");
    if (mainFramesetChildCon)
        mainFramesetChildCon.setAttribute("rows", "0,*");

    var frmHeaderChild = parent.document.getElementById("frmHeaderChild");
    if (frmHeaderChild) {
        var divHeadLoader = frmHeaderChild.contentWindow.document.getElementById("divHeadLoader");
        if (divHeadLoader) {
            divHeadLoader.style.display = "none";
        }
    }
}

function HiddenDiv(divName) {
    $("#" + divName).removeClass("dialog");
    $("#" + divName).dialog('close');
}