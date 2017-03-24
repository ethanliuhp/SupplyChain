<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ubsWebHeader.ascx.cs"
    Inherits="UserControls_ubsWebHeader" %>

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
        window.location = "Default.aspx";
    }
    function LoginOrExit(obj) {

        if (obj.innerText.indexOf("登录") > -1) {
            window.location = "Login.aspx";
        }
        else {
            document.getElementById('<%= btnExit.ClientID %>').click();
        }
    }
</script>

<table style="width: 100%; height: 40px;">
    <tr>
        <td class="header_td_logo">
        </td>
        <td class="header_td_bg">
            <label title="主页" id="linkMainPage" runat="server" class="header_link_font" onclick="return LinkMainPage();">
                主页</label>
            &nbsp;|&nbsp; <span id="pnlUserInfo" runat="server" visible="false">
                <label id="lblUserInfo" runat="server" class="header_link_font">
                </label>
                &nbsp;|&nbsp;
                <label title="修改密码" id="lblUpdatePwd" runat="server" class="header_link_font">
                    修改密码
                </label>
                &nbsp;|&nbsp;</span>
            <label id="lblLoginOrExit" runat="server" class="header_link_font" onclick="return LoginOrExit(this);">
                登录
            </label>
            <div style="display: none;">
                <asp:Button ID="btnExit" runat="server" Text="注销" OnClick="btnExit_Click" /></div>
        </td>
    </tr>
</table>
