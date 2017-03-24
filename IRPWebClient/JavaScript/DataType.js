function IsInt(sValue) {
    var sParen = "^[-,+]*\\d+$";
    return Test(sValue, sParen);
}
function IsFloat(sValue) {
    var sParen = "(^[-,+]*\\d+$)|(^[-,+]*\\d+\\.\\d+$)";
    return Test(sValue, sParen);
}
function GetInt(sValue) {
    var iReturn = null;
    if (IsInt(sValue)) {
        iReturn = parseInt(sValue);
    }
    return iReturn;
}
function IsDate(sValue) {
    var sReturn = false;
    var sParen = "^\\d{4}-([0,1]{0,1})(\\d{1})-([0,1,2,3]{0,1})(\\d{1})$";
    if (sValue != null && sValue != NaN) {
        if (Test(sValue, sParen)) {
            var arr = sValue.split("-");
            var year = arr[0];
            var month = arr[1];
            var day = arr[2];
            if (day < 32 && month < 13) {
                return true;
            }
        }
    }
    return false;
}
function GetFloat(sValue) {
    var fReturn = null;
    if (IsFloat(sValue)) {
        fReturn = parseInt(sValue);
    }
    return fReturn;
}
function Test(sValue, sParen) {
    var re = RegExp(sParen);
    return re.test(sValue);
}
function GetDate(sValue) {
    var newDate = null;
    if (IsDate(sValue)) {
        var arys = new Array();
        arys = str.split('-');
        newDate = new Date(arys[0], arys[1], arys[2]);
    }
    return newDate;
}  