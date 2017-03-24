function CreateDOM() { var dom = new ActiveXObject("Microsoft.XMLDOM"); dom.async = false; return dom; }

function CreateXMLHTTP() { var arrSignatures = ["MSXML2.XMLHTTP.5.0", "MSXML2.XMLHTTP.4.0", "MSXML2.XMLHTTP.3.0", "MSXML2.XMLHTTP", "Microsoft.XMLHTTP"]; for (var i = 0; i < arrSignatures.length; i++) { try { var xmlHttp = new ActiveXObject(arrSignatures[i]); if (xmlHttp) { break; } } catch (ex) { if (i < arrSignatures.length) { continue; } else { break; } } } if (!xmlHttp && typeof XMLHttpRequest != "undefined") { xmlHttp = new XMLHttpRequest(); } return xmlHttp; }

//function tbPostalCode_onkeydown() { if ((this.event.keyCode < 48 || this.event.keyCode > 57) && this.event.keyCode != 8) { this.event.returnValue = false; } }

function tbMoney_onkeydown() { if ((this.event.keyCode < 48 || this.event.keyCode > 57) && this.event.keyCode != 8) { this.event.returnValue = false; } }

function tbAllCode_onkeydown() { if (this.event.keyCode != 9) { this.event.returnValue = false; } }

//支持大小键盘、tab键和回格
function tbPostalCode_onkeydown() {
    if ((this.event.keyCode >= 48 && this.event.keyCode <= 57) || (this.event.keyCode >= 96 && this.event.keyCode <= 105) || this.event.keyCode == 8 || this.event.keyCode == 9)
        this.event.returnValue = true;
    else
        this.event.returnValue = false;
}

//支持大小键盘、tab键和回格支持-号和小数点
function tbPostalCode_onkeydown1(obj) {
    if ((this.event.keyCode >= 48 && this.event.keyCode <= 57) || (this.event.keyCode >= 96 && this.event.keyCode <= 105) || this.event.keyCode == 8 || this.event.keyCode == 9)
        this.event.returnValue = true;
    else if (this.event.keyCode == 189 && obj.value == "")//有减号情况
        this.event.returnValue = true;
    else if (this.event.keyCode == 190 && obj.value != "" && obj.value != "-" && obj.value.indexOf(".") == -1)    //有小数点情况
        this.event.returnValue = true;
    else if (this.event.keyCode == 229)    //有减号或小数点情况
    {
        this.event.returnValue = true;
        var charStr = obj.value.substr(obj.value.length - 1);
        if (charStr == "。") {
            if (obj.value.length == 1 || obj.value.substr(obj.value.length - 2, 1) == "-" || obj.value.substr(obj.value.length - 2, 1) == "+")
                obj.value = obj.value.replace("。", "");
            else
                obj.value = obj.value.replace("。", ".");
        }
        else if (charStr == "-" || charStr == "+") {
            if (obj.value.length > 1) {
                obj.value = obj.value.substr(0, obj.value.length - 1);
            }
        }
        else {
            obj.value = obj.value.substr(0, obj.value.length - 1);
        }
    }
    else
        this.event.returnValue = false;
}

//检验浏览器
function checkNavigator() {
    if (navigator.userAgent.indexOf("IE") > 0) {
        var versionname = navigator.appVersion.toString()
        if (navigator.userAgent.indexOf("360SE") > -1)//判断是否360浏览器
            return "360";
        else if (versionname.indexOf("MSIE 5") > -1)
            return "IE5";
        else if (versionname.indexOf("MSIE 6") > -1)
            return "IE6";
        else if (versionname.indexOf("MSIE 9") > -1)
            return "IE9";
        else if (versionname.indexOf("MSIE 8") > -1)
            return "IE8";
        else if (versionname.indexOf("MSIE 7") > -1)
            return "IE7"
        else if (versionname.indexOf('Maxthon') > -1)//傲游浏览器
            return "Maxthon";
        else if (versionname.indexOf('TencentTraveler') > -1)//判断是否TT浏览器
            return "TT";
        else
            return "IE";

    }
    if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0)//火狐
        return "Firefox";
    if (isSafari = navigator.userAgent.indexOf("Safari") > 0)
        return "Safari";
    if (isCamino = navigator.userAgent.indexOf("Camino") > 0)
        return "Camino";
    if (isMozilla = navigator.userAgent.indexOf("Gecko") > 0)
        return "Gecko";
    return false;
}

