<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ubsWebHeader.ascx.cs"
    Inherits="Main_ubsWebHeader" %>
<link href="../css/Header.css" rel="stylesheet" type="text/css" />

<script src="../JavaScript/jquery-1.10.2-vsdoc.js" type="text/javascript"></script>

<script src="../JavaScript/jquery-1.10.2.js" type="text/javascript"></script>

<script type="text/javascript">
    function ShowPersonalSettingDlg(url) {
        var result = window.showModalDialog(url, window, 'dialogWidth: 380px;dialogHeight: 280px;status:no;center: yes;resizable:on; scroll:no');
        if (result == 'OK') {
            return true;
        }
        else {
            return false;
        }
    }
    function LinkMainPage() {
        window.parent.window.location = "Default.aspx";
    }
    function LinkSystemManagement() {
        window.parent.window.location = "Admin/MainPage.aspx";
    }
    function LoginOrExit(obj) {
        document.getElementById("<%=btnExitHide.ClientID %>").click();
        if (obj.innerText.indexOf("登录") > -1) {
            window.parent.window.location = "../Login/SSOLogin.aspx";
        }
        else {
            window.parent.window.location = "../Login/SSOLogin.aspx?SignOut=true";
        }
    }
    window.onload = function() {
        show();
    }
    function show() {

        var date = new Date(); //日期对象
        var now = "";
        now = date.getFullYear() + "年"; //读英文就行了
        now = now + (date.getMonth() + 1) + "月"; //取月的时候取的是当前月-1如果想取当前月+1就可以了
        now = now + date.getDate() + "日";

        var week;
        var weekDay = date.getDay();
        if (weekDay == 0) week = "星期日"
        if (weekDay == 1) week = "星期一"
        if (weekDay == 2) week = "星期二"
        if (weekDay == 3) week = "星期三"
        if (weekDay == 4) week = "星期四"
        if (weekDay == 5) week = "星期五"
        if (weekDay == 6) week = "星期六"

        now = now + " " + week + " ";

        var hour = date.getHours() + "";
        hour = hour.length == 1 ? "0" + hour : hour
        var minutes = date.getMinutes() + "";
        minutes = minutes.length == 1 ? "0" + minutes : minutes
        var seconds = date.getSeconds() + "";
        seconds = seconds.length == 1 ? "0" + seconds : seconds

        now = now + hour + ":";
        now = now + minutes + ":";
        now = now + seconds;

        document.getElementById("lblDate").innerHTML = "今天是" + now; //div的html是now这个字符串
        setTimeout(function() { show() }, 500);
        //setInterval(show, 100); //设置过1000毫秒就是1秒，调用show方法
    }
</script>

<table style="width: 100%; height: 99%;">
    <tr>
        <td rowspan="2" class="header_td_logo">
            <img src="../images/FrameConfig/header_bg.png" />
        </td>
        <td class="header_td_bg" style=" text-align:right;">
            <span id="pnlUserInfo" runat="server" visible="false">
                <label id="lblUserInfo" runat="server">
                </label>
                &nbsp;|&nbsp;</span>
            <label id="lblLoginOrExit" runat="server" class="header_link_font" onclick="return LoginOrExit(this);">
                登录
            </label>
            <div style="display: none;">
                <asp:Button ID="btnExit" runat="server" Text="退出登录" OnClick="btnExit_Click" />
                <asp:Button ID="btnExitHide" runat="server" Text="退出登录" OnClick="btnExitHide_Click" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="header_td_Date" style=" text-align:right;">
            <label id="lblDate">
            </label>
        </td>
    </tr>
</table>
